using System.Threading.Tasks;

namespace UEx.Threading
{
    class EnsureAsyncsCompile
    {
#pragma warning disable 1998
        // Return type of void, Task or Task<int>
        private static async void DoNothingAsync()
        {
            // For Task<int> insert return 0; here
        }
#pragma warning restore 1998 
    }
}
