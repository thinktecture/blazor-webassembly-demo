using BlazorConfTool.Client.Services;
using BlazorConfTool.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorConfTool.Client.Pages
{
    public partial class Conference : ComponentBase
    {
        [Parameter]
        public string Mode { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private bool _isShow { get; set; }

        [Inject]
        private IAlertService _alert { get; set; }
        [Inject]
        private ConferencesService _conferencesService { get; set; }
        [Inject]
        private CountriesService _countriesService { get; set; }

        private ConferenceDetails _conferenceDetails;
        private List<string> _countries;

        public Conference()
        {
            _conferenceDetails = new ConferenceDetails
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow
            };

            _countries = new List<string>();
        }

        protected override async Task OnInitializedAsync()
        {
            _isShow = Mode == Modes.Show;

            switch (Mode)
            {
                case Modes.Show:
                    var conferenceResult = await _conferencesService.GetConferenceDetails(Id);
                    _conferenceDetails = conferenceResult;
                    break;
                case Modes.Edit:
                case Modes.New:
                    var countriesResult = await _countriesService.ListCountries();
                    _countries = countriesResult;
                    _conferenceDetails.Country = _countries[0];
                    break;
            }
        }

        private async Task SaveConference(EditContext editContext)
        {
            if (!await _alert.ConfirmAsync("Do you want to save this new entry?"))
            {
                Console.WriteLine("### User declined to save conference!");
                return;
            }

            await _conferencesService.AddConference(_conferenceDetails);

            Console.WriteLine("### NEW conference added...");
        }

        private async Task<IEnumerable<string>> FilterCountries(string searchText)
        {
            return await Task.FromResult(_countries.Where(
                c => c.ToLower().Contains(searchText.ToLower())).ToList());
        }
    }
}