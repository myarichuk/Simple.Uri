using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Simple.Uri.Playground
{
    [MemoryDiagnoser]
    public class Program
    {
        private static readonly string _url = "foobar://usr:pwd@hostname.com:1234/this/is/a/test?foo=bar&x=y#fragment";

        private static readonly System.Uri _uri =
            new System.Uri("foobar://usr:pwd@hostname.com:1234/this/is/a/test?foo=bar&x=y#fragment");

        [Benchmark(Baseline = true, Description = "Flurl")]
        public void Flurl()
        {
            //for (int i = 0; i < 1_000_000; i++)
            {
                _ = new Flurl.Url(_uri);
            }
        }

        [Benchmark(Description = "Simple.Uri")]
        public void SimpleUri()
        {
            //for (int i = 0; i < 1_000_000; i++)
            {
                _ = Uri.Parse(_url);
            }
        }


        static void Main(string[] args)
        {

            BenchmarkRunner.Run<Program>();
        }
    }
}
