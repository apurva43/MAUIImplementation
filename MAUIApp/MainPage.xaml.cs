namespace MAUIApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        SentrySdk.CaptureMessage("Hello Sentry");
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
        try
        {
            throw new Exception("Oops from MAUI");
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }

        if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";
        SentrySdk.CaptureMessage("User clicked on button ");
        SemanticScreenReader.Announce(CounterBtn.Text);
	}
}


