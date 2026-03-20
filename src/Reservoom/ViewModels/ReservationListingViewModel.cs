using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reservoom.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly HotelStore _hotelStore;

        private readonly ObservableCollection<ReservationViewModel> _reservations;

        public IEnumerable<ReservationViewModel> Reservations => _reservations;

        public bool HasReservations => _reservations.Any();

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));

                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ICommand LoadReservationsCommand { get; }
        public ICommand MakeReservationCommand { get; }
        public ICommand ModifyReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }

        private ReservationViewModel _selectedReservation;
        public ReservationViewModel SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        public ReservationListingViewModel(
            HotelStore hotelStore,
            NavigationService<MakeReservationViewModel> makeReservationNavigationService,
            ParameterizedNavigationService<Reservation, ModifyReservationViewModel> modifyReservationNavigationService)
        {
            _hotelStore = hotelStore;
            _reservations = new ObservableCollection<ReservationViewModel>();

            LoadReservationsCommand = new LoadReservationsCommand(this, hotelStore);
            MakeReservationCommand = new NavigateCommand<MakeReservationViewModel>(makeReservationNavigationService);
            ModifyReservationCommand = new ModifyReservationNavigateCommand(this, modifyReservationNavigationService);
            DeleteReservationCommand = new DeleteReservationCommand(this, hotelStore);

            _hotelStore.ReservationMade += OnReservationMode;
            _hotelStore.ReservationDeleted += OnReservationDeleted;
            _hotelStore.ReservationModified += OnReservationModified;
            _reservations.CollectionChanged += OnReservationsChanged;
        }

        public override void Dispose()
        {
            _hotelStore.ReservationMade -= OnReservationMode;
            _hotelStore.ReservationDeleted -= OnReservationDeleted;
            _hotelStore.ReservationModified -= OnReservationModified;
            base.Dispose();
        }

        private void OnReservationMode(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }

        private void OnReservationDeleted(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = _reservations.FirstOrDefault(
                r => r.Reservation == reservation);

            if (reservationViewModel != null)
            {
                _reservations.Remove(reservationViewModel);
            }
        }

        private void OnReservationModified(Reservation oldReservation, Reservation newReservation)
        {
            ReservationViewModel reservationViewModel = _reservations.FirstOrDefault(r => r.Reservation == oldReservation);

            if (reservationViewModel != null)
            {
                int index = _reservations.IndexOf(reservationViewModel);
                _reservations[index] = new ReservationViewModel(newReservation);
                return;
            }

            _reservations.Add(new ReservationViewModel(newReservation));
        }

        public static ReservationListingViewModel LoadViewModel(
            HotelStore hotelStore,
            NavigationService<MakeReservationViewModel> makeReservationNavigationService,
            ParameterizedNavigationService<Reservation, ModifyReservationViewModel> modifyReservationNavigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(
                hotelStore,
                makeReservationNavigationService,
                modifyReservationNavigationService);

            viewModel.LoadReservationsCommand.Execute(null);

            return viewModel;
        }

        public void UpdateReservations(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();

            foreach (Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }

        private void OnReservationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasReservations));
        }
    }
}
