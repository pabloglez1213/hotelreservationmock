using Reservoom.Stores;
using Reservoom.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Reservoom.Commands
{
    public class DeleteReservationCommand : AsyncCommandBase
    {
        private readonly ReservationListingViewModel _viewModel;
        private readonly HotelStore _hotelStore;

        public DeleteReservationCommand(ReservationListingViewModel viewModel, HotelStore hotelStore)
        {
            _viewModel = viewModel;
            _hotelStore = hotelStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReservationListingViewModel.SelectedReservation))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedReservation != null;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the reservation for room {_viewModel.SelectedReservation.RoomID} by \"{_viewModel.SelectedReservation.Username}\"?",
                "Delete Reservation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                await _hotelStore.DeleteReservation(_viewModel.SelectedReservation.Reservation);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete the reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
