using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Reservoom.ViewModels
{
    public class ModifyReservationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public Reservation OriginalReservation { get; }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));

                ClearErrors(nameof(Username));
                if (!HasUsername)
                {
                    AddError("Username cannot be empty.", nameof(Username));
                }

                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private int _floorNumber;
        public int FloorNumber
        {
            get => _floorNumber;
            set
            {
                _floorNumber = value;
                OnPropertyChanged(nameof(FloorNumber));

                ClearErrors(nameof(FloorNumber));
                if (!HasFloorNumberGreaterThanZero)
                {
                    AddError("Floor number must be greater than zero.", nameof(FloorNumber));
                }

                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private int _roomNumber;
        public int RoomNumber
        {
            get => _roomNumber;
            set
            {
                _roomNumber = value;
                OnPropertyChanged(nameof(RoomNumber));
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));

                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));
                if (!HasStartDateBeforeEndDate)
                {
                    AddError("The start date cannot be after the end date.", nameof(StartDate));
                }

                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));

                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));
                if (!HasStartDateBeforeEndDate)
                {
                    AddError("The end date cannot be before the start date.", nameof(EndDate));
                }

                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public bool CanSubmit =>
            HasUsername &&
            HasFloorNumberGreaterThanZero &&
            HasStartDateBeforeEndDate &&
            !HasErrors;

        private bool HasUsername => !string.IsNullOrEmpty(Username);
        private bool HasFloorNumberGreaterThanZero => FloorNumber > 0;
        private bool HasStartDateBeforeEndDate => StartDate < EndDate;

        private string _submitErrorMessage;
        public string SubmitErrorMessage
        {
            get => _submitErrorMessage;
            set
            {
                _submitErrorMessage = value;
                OnPropertyChanged(nameof(SubmitErrorMessage));
                OnPropertyChanged(nameof(HasSubmitErrorMessage));
            }
        }

        public bool HasSubmitErrorMessage => !string.IsNullOrEmpty(SubmitErrorMessage);

        private bool _isSubmitting;
        public bool IsSubmitting
        {
            get => _isSubmitting;
            set
            {
                _isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            }
        }

        public AsyncCommandBase SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;

        public bool HasErrors => _propertyNameToErrorsDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ModifyReservationViewModel(
            Reservation reservation,
            HotelStore hotelStore,
            NavigationService<ReservationListingViewModel> reservationListingNavigationService)
        {
            OriginalReservation = reservation;

            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();

            Username = reservation.Username;
            FloorNumber = reservation.RoomID.FloorNumber;
            RoomNumber = reservation.RoomID.RoomNumber;
            StartDate = reservation.StartTime;
            EndDate = reservation.EndTime;

            SubmitCommand = new ModifyReservationCommand(this, hotelStore, reservationListingNavigationService);
            CancelCommand = new NavigateCommand<ReservationListingViewModel>(reservationListingNavigationService);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }

        private void AddError(string errorMessage, string propertyName)
        {
            if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }

            _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void ClearErrors(string propertyName)
        {
            _propertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
