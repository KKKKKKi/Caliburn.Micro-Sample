namespace CaliburnMicroSample.Design
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            FilePath = "Sample File Path";
        }

        public string FilePath { get; private set; }
    }
}
