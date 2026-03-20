using Reservoom.Exceptions;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using Reservoom.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Reservoom.Commands
{
    public class ModifyReservationCommand : AsyncCommandBase
    {
        private readonly ModifyReservationViewModel _modifyReservationViewModel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationService<ReservationListingViewModel> _reservationListingNavigationService;

        public ModifyReservationCommand(
            ModifyReservationViewModel modifyReservationViewModel,
            HotelStore hotelStore,
            NavigationService<ReservationListingViewModel> reservationListingNavigationService)
        {
            _modifyReservationViewModel = modifyReservationViewModel;
            _hotelStore = hotelStore;
            _reservationListingNavigationService = reservationListingNavigationService;

            _modifyReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return _modifyReservationViewModel.CanSubmit && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _modifyReservationViewModel.SubmitErrorMessage = string.Empty;
            _modifyReservationViewModel.IsSubmitting = true;

            Reservation newReservation = new Reservation(
                new RoomID(_modifyReservationViewModel.FloorNumber, _modifyReservationViewModel.RoomNumber),
                _modifyReservationViewModel.Username,
                _modifyReservationViewModel.StartDate,
                _modifyReservationViewModel.EndDate);

            try
            {
                await _hotelStore.ModifyReservation(_modifyReservationViewModel.OriginalReservation, newReservation);
                _reservationListingNavigationService.Navigate();
            }
            catch (ReservationConflictException)
            {
                _modifyReservationViewModel.SubmitErrorMessage = "This room is already taken on those dates.";
            }
            catch (InvalidReservationTimeRangeException)
            {
                _modifyReservationViewModel.SubmitErrorMessage = "Start date must be before end date.";
            }
            catch (Exception)
            {
                _modifyReservationViewModel.SubmitErrorMessage = "Failed to modify reservation.";
            }

            _modifyReservationViewModel.IsSubmitting = false;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ModifyReservationViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
