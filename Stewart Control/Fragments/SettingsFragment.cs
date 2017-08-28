using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Stewart_Control
{
    public class SettingsFragment : Android.Support.V4.App.Fragment
    {
        public TextView[] mTextViews = new TextView[12];
        private Switch mSwitch1;
        private Switch mSwitch2;
        private EditText[] mMinEdits = new EditText[6];
        private EditText[] mMaxEdits = new EditText[6];
        private Button mButtonSave;

        private BluetoothConnection mConection;

        private SamplePagerAdapter parent;

        public SettingsFragment(SamplePagerAdapter par)
        {
            parent = par;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Settings, container, false);

            mTextViews[0] = view.FindViewById<TextView>(Resource.Id.sToggleText1);
            mTextViews[1] = view.FindViewById<TextView>(Resource.Id.sToggleText2);
            mTextViews[2] = view.FindViewById<TextView>(Resource.Id.sTextD);
            mTextViews[3] = view.FindViewById<TextView>(Resource.Id.sText1);
            mTextViews[4] = view.FindViewById<TextView>(Resource.Id.sText2);
            mTextViews[5] = view.FindViewById<TextView>(Resource.Id.sText3);
            mTextViews[6] = view.FindViewById<TextView>(Resource.Id.sText4);
            mTextViews[7] = view.FindViewById<TextView>(Resource.Id.sText5);
            mTextViews[8] = view.FindViewById<TextView>(Resource.Id.sText6);
            mTextViews[9] = view.FindViewById<TextView>(Resource.Id.sText7);
            mTextViews[10] = view.FindViewById<TextView>(Resource.Id.sText8);
            mTextViews[11] = view.FindViewById<TextView>(Resource.Id.sText9);
            foreach (TextView txt in mTextViews)
                txt.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mSwitch1 = view.FindViewById<Switch>(Resource.Id.sToggle1);
            mSwitch2 = view.FindViewById<Switch>(Resource.Id.sToggle2);
            mSwitch1.CheckedChange += MSwitch1_CheckedChange;
            mSwitch2.CheckedChange += MSwitch2_CheckedChange;
            
            mButtonSave = view.FindViewById<Button>(Resource.Id.sButtonSave);
            mButtonSave.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mButtonSave.SetBackgroundColor(new Android.Graphics.Color(0xC0, 0xC0, 0xC0));
            mButtonSave.Click += MButtonSave_Click;

            mMinEdits[0] = view.FindViewById<EditText>(Resource.Id.sMinX);
            mMinEdits[1] = view.FindViewById<EditText>(Resource.Id.sMinY);
            mMinEdits[2] = view.FindViewById<EditText>(Resource.Id.sMinZ);
            mMinEdits[3] = view.FindViewById<EditText>(Resource.Id.sMinR);
            mMinEdits[4] = view.FindViewById<EditText>(Resource.Id.sMinP);
            mMinEdits[5] = view.FindViewById<EditText>(Resource.Id.sMinYaw);

            mMaxEdits[0] = view.FindViewById<EditText>(Resource.Id.sMaxX);
            mMaxEdits[1] = view.FindViewById<EditText>(Resource.Id.sMaxY);
            mMaxEdits[2] = view.FindViewById<EditText>(Resource.Id.sMaxZ);
            mMaxEdits[3] = view.FindViewById<EditText>(Resource.Id.sMaxR);
            mMaxEdits[4] = view.FindViewById<EditText>(Resource.Id.sMaxP);
            mMaxEdits[5] = view.FindViewById<EditText>(Resource.Id.sMaxYaw);
            foreach (EditText txt in mMinEdits)
            {
                txt.SetTextColor(new Android.Graphics.Color(0, 0, 0));
                txt.SetRawInputType(Android.Text.InputTypes.ClassText);
            }
                
            foreach (EditText txt in mMaxEdits)
            {
                txt.SetTextColor(new Android.Graphics.Color(0, 0, 0));
                txt.SetRawInputType(Android.Text.InputTypes.ClassText);
            }

            SetEditsSynchronized();

            return view;
        }

        private void MSwitch2_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {

            if (mSwitch2.Checked == true)
            {

                parent.parent.CreateListenerThread();
                mTextViews[1].Text = "Reciever Active!";
            }
            else
            {
                parent.parent.CloseListenerThread();
                mTextViews[1].Text = "Reciever Deactive...";
            }

        }

        private void MSwitch1_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {

            if (mSwitch1.Checked == true)
            {
                if (parent.parent.mBTconnected == false)
                {
                    mTextViews[0].Text = "Bluetooth connect...";
                    parent.parent.CreateBluetoothConnection();
                }

            }
            else
            {
                if (parent.parent.mBTconnected == true)
                {
                    mTextViews[0].Text = "Bluetooth disconnect...";
                    parent.parent.CloseBluetoothConnection();
                }
            }
        }

        private void SetEditsSynchronized()
        {
            InverseParams ip = parent.parent.mInverseParams;
            mMinEdits[0].Text = ip.XYZrangeMin[0].ToString();
            mMinEdits[1].Text = ip.XYZrangeMin[1].ToString();
            mMinEdits[2].Text = ip.XYZrangeMin[2].ToString();
            mMinEdits[3].Text = ip.ABCrangeMin[0].ToString();
            mMinEdits[4].Text = ip.ABCrangeMin[1].ToString();
            mMinEdits[5].Text = ip.ABCrangeMin[2].ToString();

            mMaxEdits[0].Text = ip.XYZrangeMax[0].ToString();
            mMaxEdits[1].Text = ip.XYZrangeMax[1].ToString();
            mMaxEdits[2].Text = ip.XYZrangeMax[2].ToString();
            mMaxEdits[3].Text = ip.ABCrangeMax[0].ToString();
            mMaxEdits[4].Text = ip.ABCrangeMax[1].ToString();
            mMaxEdits[5].Text = ip.ABCrangeMax[2].ToString();
        }

        private void MButtonSave_Click(object sender, System.EventArgs e)
        {
            InverseParams ip = parent.parent.mInverseParams;
            try
            {
                ip.XYZrangeMin[0] = float.Parse(mMinEdits[0].Text);
                ip.XYZrangeMin[1] = float.Parse(mMinEdits[1].Text);
                ip.XYZrangeMin[2] = float.Parse(mMinEdits[2].Text);
                ip.ABCrangeMin[0] = float.Parse(mMinEdits[3].Text);
                ip.ABCrangeMin[1] = float.Parse(mMinEdits[4].Text);
                ip.ABCrangeMin[2] = float.Parse(mMinEdits[5].Text);

                ip.XYZrangeMax[0] = float.Parse(mMaxEdits[0].Text);
                ip.XYZrangeMax[1] = float.Parse(mMaxEdits[1].Text);
                ip.XYZrangeMax[2] = float.Parse(mMaxEdits[2].Text);
                ip.ABCrangeMax[0] = float.Parse(mMaxEdits[3].Text);
                ip.ABCrangeMax[1] = float.Parse(mMaxEdits[4].Text);
                ip.ABCrangeMax[2] = float.Parse(mMaxEdits[5].Text); 

            }
            catch
            {
                SetEditsSynchronized();
            }
            

        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Settings";
        }
    }

}