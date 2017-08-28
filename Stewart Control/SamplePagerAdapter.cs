using Android.Support.V4.App;
using System.Collections.Generic;

namespace Stewart_Control
{
    public class SamplePagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public MainActivity parent;

        public SamplePagerAdapter(Android.Support.V4.App.FragmentManager fragManager, MainActivity par) : base(fragManager)
        {
            parent = par;

            mFragmentHolder = new List<Fragment>();
            mFragmentHolder.Add(new SettingsFragment(this));
            mFragmentHolder.Add(new InverseFragment(this));
            mFragmentHolder.Add(new AccelerometerFragment(this));
            mFragmentHolder.Add(new TargetFragment(this));
        }

        public override int Count
        {
            get { return mFragmentHolder.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentHolder[position];
        }
    }

}