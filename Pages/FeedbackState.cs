using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace FeedbackApp
{
    public class FeedbackState
    {
        private readonly IJSRuntime jsRuntime;

        public FeedbackState(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task SaveFeedbackAsync(List<Feedback> feedbackList)
        {
            var json = JsonSerializer.Serialize(feedbackList);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "feedback", json);
        }

        public async Task<List<Feedback>> LoadFeedbackAsync()
        {
            var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "feedback");

            if (String.IsNullOrEmpty(json))
            {
                return new List<Feedback>();
            }

            var feedbackList = JsonSerializer.Deserialize<List<Feedback>>(json);
            return feedbackList ?? new List<Feedback>();
        }
    }

}