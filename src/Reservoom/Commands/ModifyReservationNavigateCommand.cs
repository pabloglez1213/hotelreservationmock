using Reservoom.Models;
using Reservoom.Services;
using Reservoom.ViewModels;
using System.ComponentModel;

namespace Reservoom.Commands
{
    public class ModifyReservationNavigateCommand : CommandBase
    {
        private readonly ReservationListingViewModel _reservationListingViewModel;
        private readonly ParameterizedNavigationService<Reservation, ModifyReservationViewModel> _modifyReservationNavigationService;

        public ModifyReservationNavigateCommand(
            ReservationListingViewModel reservationListingViewModel,
            ParameterizedNavigationService<Reservation, ModifyReservationViewModel> modifyReservationNavigationService)
        {
            _reservationListingViewModel = reservationListingViewModel;
            _modifyReservationNavigationService = modifyReservationNavigationService;

            _reservationListingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && _reservationListingViewModel.SelectedReservation != null;
        }

        public override void Execute(object parameter)
        {
            _modifyReservationNavigationService.Navigate(_reservationListingViewModel.SelectedReservation.Reservation);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReservationListingViewModel.SelectedReservation))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
