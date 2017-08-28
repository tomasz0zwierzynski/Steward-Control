using Android.Hardware;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace Stewart_Control
{
    public delegate void SensorHandler(SensorEvent e);

    public class AccelerometerFragment : Android.Support.V4.App.Fragment
    {
        private TextView mTextView1;
        private Button mButton;
        private TextView mTextView2;

        private double pitchGain = 0.6F;
        private double rollGain = 0.6F;
        private double tPitch;
        private double tRoll;

        private bool transmiting;

        private SamplePagerAdapter parent;

        public AccelerometerFragment(SamplePagerAdapter par)
        {
            parent = par;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Accelerometer, container, false);
            
            mTextView1 = view.FindViewById<TextView>(Resource.Id.textView1a);
            mTextView1.SetTextColor(new Android.Graphics.Color(0,0,0));
            mTextView1.Text = "Set device horizontally and press Start";

            mTextView2 = view.FindViewById<TextView>(Resource.Id.textView2a);
            mTextView2.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mTextView2.Text = "Pitch: - \nRoll: - ";

            mButton = view.FindViewById<Button>(Resource.Id.button1a);
            mButton.Text = "Start";
            mButton.SetBackgroundColor(new Android.Graphics.Color(0xC0, 0xC0, 0xC0));
            mButton.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mButton.Click += delegate
            {
                transmiting = true;
                mTextView1.Text = "Rotate device to control the platform.";
            };

            transmiting = false;

            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Accelerometer";
        }

        public void OnAccelerometerData(SensorEvent e)
        {
            if (transmiting == true)
            {
                double x = e.Values[0];
                double y = e.Values[1];
                double z = e.Values[2];

                double roll = Math.Atan(x / Math.Sqrt(Math.Pow(z, 2) + Math.Pow(y, 2)));
                double pitch = Math.Atan(y / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2)));

                roll *= 180.0 / Math.PI;
                pitch *= 180.0 / Math.PI;

                tRoll = roll * rollGain;
                tPitch = pitch * pitchGain;

                if (parent.parent.mBluetoothConnection != null)
                {
                    //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setPitch, (float)tPitch);
                    parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setPitch, (float)tPitch));

                    //byte[] msg2 = CommandProtocol.NewSimple(CommandProtocol.Cmd.setRoll, (float)tRoll);
                    parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setRoll, (float)tRoll));
                }

                mTextView2.Text = string.Format("Pitch: {0:f}, Roll: {1:f}", tPitch, tRoll);
            }    
        }
        
        public void OnPageChanged(object sender, System.EventArgs args)
        {
            //Turn off control
            transmiting = false;
            mTextView1.Text = "Set device horizontally. ";
            mTextView2.Text = "Pitch: -  Roll: - ";
        }

    }
    
}