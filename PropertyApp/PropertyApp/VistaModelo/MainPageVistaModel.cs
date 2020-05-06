using MvvmHelpers;
using PropertyApp.Modelo;
using PropertyApp.Servicio;
using PropertyApp.url;
using PropertyApp.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WSGOPLAY.Models.red;
using Xamarin.Forms;

namespace PropertyApp.VistaModelo
{
    public class MainPageVistaModel : BaseViewModel
    {
        //Servicio de canchas
        CanchasServicio CanchasServicio;
        GoPlayServicio GoPlayServicio;

        private Boolean isOcupado;

        public Boolean IsOcupado
        {
            get { return isOcupado; }
            set { isOcupado = value; }
        }

        private string resultadoBusqueda;

        public string ResultadoBusqueda
        {
            get => resultadoBusqueda;
            set => SetProperty(ref resultadoBusqueda, value);
        }

        private bool isLogueado;

        public bool IsLogueado
        {
            get => isLogueado;
            set => SetProperty(ref isLogueado, value);
        }

        private bool isLogueadoNo;

        public bool IsLogueadoNO
        {
            get => isLogueadoNo;
            set => SetProperty(ref isLogueadoNo, value);
        }

        private ObservableCollection<CanchasModel> canchas;
        public ObservableCollection<CanchasModel> Canchas
        {
            get => canchas;
            set => SetProperty(ref canchas, value);
        }

        public string Name { get; set; }
        public List<PropertyType> PropertyTypeList => GetPropertyTypes();

        private ObservableCollection<Property> propertyList;

        public ObservableCollection<Property> PropertyList
        {
            get => propertyList;
            set => SetProperty(ref propertyList, value);
        }

        public ICommand BuscarCanchaCommando => new Command<string>(async (string buscar) =>
       {
           if (string.IsNullOrEmpty(buscar))
           {
               Canchas = await GoPlayServicio.GetDatosAsync<CanchasModel>(Url.urlPages);
               PropertyList = CargarCanchas(Canchas);
               return;
           }

           isOcupado = true;
           Canchas = new ObservableCollection<CanchasModel>();
           Canchas = await GoPlayServicio.GetDatosAsync<CanchasModel>(Url.urlPagesBuscar + buscar);
           PropertyList = null;
           if (canchas.Count > 0)
           {
               ResultadoBusqueda = string.Empty;
               PropertyList = CargarCanchas(Canchas);
           }
           else
               ResultadoBusqueda = "No se han encontrado resultados";
           isOcupado = false;

       });

        public ICommand IniciarCommand => new Command( async () =>
        {
            await  Application.Current.MainPage.Navigation.PushModalAsync(new LoginView { BindingContext = this });
        }); 
        public ICommand RefrescarComando => new Command( async () =>
        {
            await Application.Current.MainPage.DisplayAlert("Refrescar", "Refres", "OK");

        });

        public MainPageVistaModel()
        {
           
            IsOcupado = true;
            GoPlayServicio = new GoPlayServicio();
            PropertyList = new ObservableCollection<Property>();
            Load();
            IsOcupado = false;
        }
        private List<PropertyType> GetPropertyTypes()
        {
            return new List<PropertyType>
            {
                new PropertyType { TypeName = "Todas", Name="todas" },
                new PropertyType { TypeName = "Favorita", Name="favorita" },
                new PropertyType { TypeName = "Recomendada" , Name="recomendada"},
                new PropertyType { TypeName = "Mi equipo", Name="miequipo" },
                new PropertyType { TypeName = "Reservas", Name="todasReservas" },
                new PropertyType { TypeName = "Partidos", Name="partidos" }
            };
        }

        private async void Load()
        {
            try
            {
                IsOcupado = true;
                var personList = await App.SQLiteDb.GetItemsAsync();
                if (personList.Count >0)
                {
                    IsLogueado = false;
                    IsLogueadoNO = true;
                    foreach (var item in personList)
                    {
                        var usuario = new ObservableCollection<WoUsers>();
                        usuario.Add(new WoUsers { Username = item.Name });
                        VistaModelo.LoginViewModel.Usuarios = usuario.First();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("...", "Para mejorar la experiencia en GoPlay debe iniciar sesión", "OK");
                    IsLogueado = true;
                    IsLogueadoNO = false;
                }

                Canchas = await GoPlayServicio.GetDatosAsync<CanchasModel>(Url.urlPages);             
                PropertyList = CargarCanchas(Canchas);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Main", ex.Message, "OK");
            }
            
        }

        private ObservableCollection<Property> CargarCanchas( ObservableCollection<CanchasModel> canchas)
        {
            var y = new ObservableCollection<Property>();
            if (canchas.Count > 0)
            {
                foreach (var item in canchas)
                {
                    y.Add(
                        new Property
                        {
                            Image = item.Avatar,
                            Address = item.Address,
                            Location = item.Registered,
                            Price = item.PageTitle,
                            Bed = item.Phone,
                            Bath = "",
                            Space = item.Address,
                            Details = item.PageDescription
                        });
                }
            }
            return y;
        }

    }
}
