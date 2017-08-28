using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace Stewart_Control
{
    public class InverseFragment : Android.Support.V4.App.Fragment
    {
        private TextView mTitle;
        private SeekBar[] seekBars = new SeekBar[6];
        private TextView[] textViews = new TextView[6];
        private LinearLayout mLinearLayout;
        private Button mButtonZero;

        private InverseParams ranges;
        private float[] real_values;
        
        private SamplePagerAdapter parent;

        public InverseFragment(SamplePagerAdapter par)
        {
            parent = par;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Inverse, container, false);

            mTitle = view.FindViewById<TextView>(Resource.Id.textViewTitle);
            mTitle.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mButtonZero = view.FindViewById<Button>(Resource.Id.button1i);
            mButtonZero.SetBackgroundColor(new Android.Graphics.Color(0xC0, 0xC0, 0xC0));
            mButtonZero.SetTextColor(new Android.Graphics.Color(0, 0, 0));

            mLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearlayouti);

            real_values = new float[6];

            int tIndex = 0, bIndex = 0;

            for (int i = 0; i < mLinearLayout.ChildCount; i++)
            {
                var childView = mLinearLayout.GetChildAt(i);

                if (childView is SeekBar)
                {
                    seekBars[bIndex] = (SeekBar)childView;
                    seekBars[bIndex].Progress = 50;
                    bIndex++;
                }
                else if (childView is TextView)
                {
                    textViews[tIndex] = (TextView)childView;
                    tIndex++;
                }
            }

            try
            {
                for (int i = 0; i < textViews.Length; i++)
                {
                    textViews[i].SetTextColor(new Android.Graphics.Color(0, 0, 0));
                }
            } catch {   }

            //Adding behavior to previously mensioned button
            mButtonZero.Click += delegate
            {
                for (int i = 0; i < seekBars.Length; i++)
                {
                    seekBars[i].Progress = 50;
                }
            };
            
            ranges = parent.parent.mInverseParams;
            
            for (int i=0; i < seekBars.Length; i++ )
            {
                seekBars[i].ProgressChanged += OnSliderMoved;
            }
            seekBars[0].ProgressChanged += OnX;
            seekBars[1].ProgressChanged += OnY;
            seekBars[2].ProgressChanged += OnZ;
            seekBars[3].ProgressChanged += OnR;
            seekBars[4].ProgressChanged += OnP;
            seekBars[5].ProgressChanged += OnYaw;
            
            return view;
        }

        private void OnYaw(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setYaw, real_values[5]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setYaw, real_values[5]));
            textViews[5].Text = String.Format("Yaw = {0:0.##} [deg]", real_values[5].ToString());
        }

        private void OnP(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setPitch, real_values[4]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setPitch, real_values[4]));
            textViews[4].Text = String.Format("Pitch = {0:0.##} [deg]", real_values[4].ToString());
        }

        private void OnR(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setRoll, real_values[3]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setRoll, real_values[3]));
            textViews[3].Text = String.Format("Roll = {0:0.##} [deg]", real_values[3].ToString());
        }

        private void OnZ(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setZ, real_values[2]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setZ, real_values[2]));
            textViews[2].Text = String.Format("Z = {0:0.##} [mm]", real_values[2].ToString());
        }

        private void OnY(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setY, real_values[1]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setY, real_values[1]));
            textViews[1].Text = String.Format("Y = {0:0.##} [mm]", real_values[1].ToString());
        }

        private void OnX(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //byte[] msg = CommandProtocol.NewSimple(CommandProtocol.Cmd.setX, real_values[0]);
            if (parent.parent.mBluetoothConnection != null)
                parent.parent.mBluetoothConnection.SendMessage(CommandProtocol.NewSimple(CommandProtocol.Cmd.setX, real_values[0]));
            textViews[0].Text = String.Format("X = {0:0.##} [mm]", real_values[0].ToString());
            textViews[0].Text = System.Text.Encoding.ASCII.GetString(CommandProtocol.NewSimple(CommandProtocol.Cmd.setX, real_values[0]));
        }

        public void OnInverseParamsChanged(InverseParams ip)
        {
            ranges = ip;
        }

        public void OnSliderMoved(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int[] percentage_value = new int[6];
            for (int i = 0; i < seekBars.Length; i++)
            {
                percentage_value[i] = seekBars[i].Progress;
            }

            real_values = CalculateRealValues(percentage_value);
            /*
            //Displaying Values
            textViews[0].Text = String.Format("X = {0:0.##} [mm]",real_values[0].ToString());
            textViews[1].Text = String.Format("Y = {0:0.##} [mm]", real_values[1].ToString());
            textViews[2].Text = String.Format("Z = {0:0.##} [mm]", real_values[2].ToString());
            textViews[3].Text = String.Format("Roll = {0:0.##} [deg]", real_values[3].ToString());
            textViews[4].Text = String.Format("Pitch = {0:0.##} [deg]", real_values[4].ToString());
            textViews[5].Text = String.Format("Yaw = {0:0.##} [deg]", real_values[5].ToString());
            */
        }

        private float[] CalculateRealValues(int[] percentage)
        {
            float[] value = new float[6];
            // XYZ:
            for (int i=0; i<3;i++)
            {
                value[i] =  (float) (0.01 * (ranges.XYZrangeMax[i] - ranges.XYZrangeMin[i]) * percentage[i] + ranges.XYZrangeMin[i]);
                value[i] = (float)decimal.Round((decimal)value[i], 2);
            }
                
            //ABC:
            for (int i=0; i<3; i++)
            {
                value[i + 3] = (float) (0.01 * (ranges.ABCrangeMax[i] - ranges.ABCrangeMin[i]) * percentage[i + 3] + ranges.ABCrangeMin[i]);
                value[i + 3] = (float)decimal.Round((decimal)value[i + 3], 2);
            }
                

            return value;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Inverse";
        }
    }

}