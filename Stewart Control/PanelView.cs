using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Stewart_Control
{
    public delegate void float2Signal(float f1, float f2);

    class PanelView : View
    {
        Context mContext;
        PositionPointsData mData;

        public const int PADDING_TOP = 5;
        public const int PADDING_SIDE = 150;
        
        //Zdarzenia
        public event MotionHandler ScreenTouched;
        public event float2Signal TargetChanged;

        public PanelView(Context context) :
			base(context)
			{
            init(context);

        }
        public PanelView(Context context, IAttributeSet attrs) :
			base(context, attrs)
			{
            init(context);
        }

        public PanelView(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
			{
            init(context);
        }

        private void init(Context ctx)
        {
            mContext = ctx;

            mData = new PositionPointsData();
        }

        protected override void OnDraw(Canvas canvas)
        {
            int screenWidth = Resources.DisplayMetrics.WidthPixels;
            int screenHeight = Resources.DisplayMetrics.HeightPixels;

            var paint = new Paint();
            paint.SetARGB(255, 156, 156, 156);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 4;
            canvas.DrawRect(new Rect(0 + PADDING_SIDE, 0 + PADDING_TOP, screenWidth - PADDING_SIDE, screenWidth - 2 * PADDING_SIDE), paint);

            paint.SetARGB(255, 067, 067, 255);
            canvas.DrawCircle(mData.TargetX, mData.TargetY, 10, paint);

            paint.SetARGB(255, 067, 255, 067);
            canvas.DrawCircle(mData.CurrentX, mData.CurrentY, 10, paint);
        }

        public void SetCurrentPosition(int currentXpercent, int currentYpercent)
        {
            mData.CurrentX = XPercentToAbsolute(currentXpercent);
            mData.CurrentY = YPercentToAbsolute(currentYpercent);

            Invalidate();
        }

        public void SetTargetPosition(float targetXpercent, float targetYpercent)
        {
            mData.TargetX = XPercentToAbsolute(targetXpercent);
            mData.TargetY = YPercentToAbsolute(targetYpercent);

            TargetChanged(targetXpercent,targetYpercent);

            Invalidate();
        }

        //public OnNewBluetoothDataEvent(new data)
        //{
        // mData.CurrentX = ...
        //}

        public override bool OnTouchEvent(MotionEvent e)
        {
            /*if (e.Action == MotionEventActions.Move)
            {
                mData.TargetX = e.RawX;
                mData.TargetY = e.RawY;

                Invalidate();
            }
            */

            ScreenTouched(e);

            mData.TargetX = e.GetX();
            mData.TargetY = e.GetY();

            TargetChanged(XAbsoluteToPercent(mData.TargetX), YAbsoluteToPercent(mData.TargetY));

            Invalidate();
            return base.OnTouchEvent(e);
        }

        public float XAbsoluteToPercent(float position)
        {
            return 100*(position - PADDING_SIDE) / (Resources.DisplayMetrics.WidthPixels - 2*PADDING_SIDE);
        }

        public float YAbsoluteToPercent(float position)
        {
            return 100 * (position - PADDING_TOP) / (Resources.DisplayMetrics.WidthPixels - 2 * PADDING_SIDE);
        }

        public float XPercentToAbsolute(float percent)
        {
            return (percent / 100) * (Resources.DisplayMetrics.WidthPixels - 2* PADDING_SIDE) + PADDING_SIDE;
        }

        public float YPercentToAbsolute(float percent)
        {
            return (percent / 100) * (Resources.DisplayMetrics.WidthPixels - 2 * PADDING_SIDE) + PADDING_TOP;
        }

    }

    class PositionPointsData
    {
        public float TargetX;
        public float TargetY;
        public float CurrentX;
        public float CurrentY;

        public PositionPointsData()
        {
            TargetX = 0;
            TargetY = 0;
            CurrentX = 0;
            CurrentY = 0;
        }
    }

}