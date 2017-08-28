using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace Stewart_Control
{
    public delegate void MotionHandler(MotionEvent e);

    public class TargetFragment : Android.Support.V4.App.Fragment
    {
        //References to Layout controls
        private TextView mTextView;
        private TextView mTextTitleView;
        private TextView mTextNormView;
        private PanelView mPanelView;
        private LinearLayout mLinearLayout;
        private TextView mTextX;
        private TextView mTextY;
        private SeekBar mSeekX;
        private SeekBar mSeekY;

        private SamplePagerAdapter parent;

        public TargetFragment(SamplePagerAdapter par)
        {
            parent = par;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Target, container, false);

            mTextTitleView = view.FindViewById<TextView>(Resource.Id.textView0);
            mTextTitleView.SetTextColor(new Android.Graphics.Color(0, 0, 0));

            mTextNormView = view.FindViewById<TextView>(Resource.Id.textView2);
            mTextNormView.SetTextColor(new Android.Graphics.Color(0, 0, 0));

            mTextView = view.FindViewById<TextView>(Resource.Id.textView1);
            mTextView.SetTextColor(new Android.Graphics.Color(0, 0, 0));

            mPanelView = view.FindViewById<PanelView>(Resource.Id.panelView1);
            mPanelView.ScreenTouched += OnPanelTouchEvent;
            mPanelView.TargetChanged += OnTargetChanged;

            mLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            mLinearLayout.SetMinimumHeight(Resources.DisplayMetrics.WidthPixels + 100);
            mLinearLayout.SetMinimumHeight(10);

            mTextX = view.FindViewById<TextView>(Resource.Id.textViewX);
            mTextX.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mTextY = view.FindViewById<TextView>(Resource.Id.textViewY);
            mTextY.SetTextColor(new Android.Graphics.Color(0, 0, 0));
            mSeekX = view.FindViewById<SeekBar>(Resource.Id.seekBarX);
            mSeekY = view.FindViewById<SeekBar>(Resource.Id.seekBarY);

            mSeekX.ProgressChanged += OnSeekBarChanged;
            mSeekY.ProgressChanged += OnSeekBarChanged;

            mTextView.Text = "Set target XY position";
            mTextNormView.Text = " ";

            return view;
        }

        public void OnPanelTouchEvent(MotionEvent e)
        {
            //Obsługa przyciśnięcia rzutu platformy

            float PosX = e.GetX();
            float PosY = e.GetY();

            float relativeX = PosX - PanelView.PADDING_SIDE;
            float relativeY = PosY - PanelView.PADDING_TOP;

            //float percentX = 100 * relativeX / (Resources.DisplayMetrics.WidthPixels - 2 * PanelView.PADDING_SIDE);
            //float percentY = 100 * relativeY / (Resources.DisplayMetrics.WidthPixels - 2 * PanelView.PADDING_SIDE);

            float percentX = mPanelView.XAbsoluteToPercent(PosX);
            float percentY = mPanelView.YAbsoluteToPercent(PosY);

            //mTextView.Text = "Pos X: " + relativeX.ToString() + " pos Y: " + relativeY.ToString(); 
            //mTextNormView.Text = "Pos %X: " + percentX.ToString() + " pos %Y: " + percentY.ToString();

            mTextX.Text = percentX.ToString();
            mTextY.Text = percentY.ToString();
        }

        public void OnTargetChanged(float x, float y)
        {
            if (x >= 0 && x <= 100)
                mSeekX.Progress = (int)Math.Round(x);
            if (y >= 0 && y <= 100)
                mSeekY.Progress = (int)Math.Round(y);
        }

        public void OnSeekBarChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //Odczyt z seekbar
            float posX = mSeekX.Progress;
            float posY = mSeekY.Progress;

            mTextX.Text = posX.ToString();
            mTextY.Text = posY.ToString();

            mPanelView.SetTargetPosition(posX, posY);
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Target";
        }
    }

}