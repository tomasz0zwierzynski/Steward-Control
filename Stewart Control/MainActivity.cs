using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Content.PM;
using Android.Hardware;
using Android.Content;

namespace Stewart_Control
{
    [Activity(Label = "Stewart Control", MainLauncher = true, Icon = "@drawable/xs", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity, ISensorEventListener
    {
        //Fields handling Tab View
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        private SamplePagerAdapter mPagerAdapter;

        private int currentPage; //0 - Settings; 1 - Inverse; 2 - Accelerometer; 3 - Target
        private TargetFragment mTargetFragment;
        private SettingsFragment mSettingsFragment;
        private InverseFragment mInverseFragment;
        private AccelerometerFragment mAccelerometerFragment;

        public InverseParams mInverseParams;

        //Fields handling bluetooth connection
        private const int BUFFER_SIZE = 255;
        public BluetoothConnection mBluetoothConnection;
        public bool mBTconnected;
        public bool mRecieveActived;

        //Fields handling accelerometer sensor
        static readonly object syncLock = new object();
        SensorManager sensorManager;
        public event SensorHandler NewAccelerometerData;
        public event System.EventHandler aPageChanged;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            mViewPager.Adapter = new SamplePagerAdapter(SupportFragmentManager, this);
            mScrollView.ViewPager = mViewPager;
            mPagerAdapter = (SamplePagerAdapter)mViewPager.Adapter;
            mSettingsFragment = (SettingsFragment)mPagerAdapter.GetItem(0);
            mInverseFragment = (InverseFragment)mPagerAdapter.GetItem(1);
            mAccelerometerFragment = (AccelerometerFragment)mPagerAdapter.GetItem(2);
            mTargetFragment = (TargetFragment)mPagerAdapter.GetItem(3);

            //Set Up Default Inverse Params (geometrical)
            SetDefaultInverseParams();

            //Set update event, when tab change
            mScrollView.NewPageSelected += OnPageChanged;

            //Set up accelerometer readings
            sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            NewAccelerometerData += mAccelerometerFragment.OnAccelerometerData;
            aPageChanged += mAccelerometerFragment.OnPageChanged;
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public void OnPageChanged(int i)
        {
            currentPage = i;
            if (currentPage == 2)
            {
                aPageChanged(this, new System.EventArgs());
            }
        }

        //Methods which are handling BT connection
        public void CreateBluetoothConnection()
        {
            try
            {
                mBluetoothConnection = new BluetoothConnection(this);
                mBTconnected = true;
            }
            catch (System.Exception ex)
            {
                mSettingsFragment.mTextViews[0].Text = ex.Message;
                mBluetoothConnection = null;
            }
         
        }

        public void CreateListenerThread()
        {

        }

        public void CloseBluetoothConnection()
        {
            try
            {
                mBluetoothConnection._socket.Close();
            }catch (System.Exception ex)
            {
                mSettingsFragment.mTextViews[0].Text = ex.Message;
            }finally
            {
                mBluetoothConnection = null;
                mBTconnected = false;
            }
            
        }

        public void CloseListenerThread()
        {

        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (syncLock)
            {
                if (currentPage == 2)
                {
                    NewAccelerometerData(e);
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
        }

        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }
        
        public void SetDefaultInverseParams()
        {
            //Like signal GetParams and Parent then sets params in ranges or something like that
            double[] min_xyz = new double[3] { -20, -20, -20 };
            double[] max_xyz = new double[3] { 20, 20, 20 };
            double[] min_abc = new double[3] { -30, -30, -30 };
            double[] max_abc = new double[3] { 30, 30, 30 };
            mInverseParams = new InverseParams(min_xyz, max_xyz, min_abc, max_abc);

        }

    }
}

