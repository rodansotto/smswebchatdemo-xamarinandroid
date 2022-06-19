using Android.App;
using Android.Widget;
using Android.OS;

namespace MySMSWebChat
{
    [Activity(Label = "My SMS Web Chat", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button buttonOK = FindViewById<Button>(Resource.Id.buttonOK);
            buttonOK.Click += ButtonOK_Click;
        }

        private void ButtonOK_Click(object sender, System.EventArgs e)
        {
            // this will not only close the activity but remove it from running at the background
            FinishAndRemoveTask();
        }
    }
}

