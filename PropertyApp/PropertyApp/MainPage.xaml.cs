using PlacesApp.ViewModels.Equipo;
using PropertyApp.Modelo;
using PropertyApp.Servicio;
using PropertyApp.url;
using PropertyApp.VistaModelo;
using PropertyApp.Vistas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PropertyApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //MainPageVistaModel MainPageVistaModel;
        MainPageVistaModel contexto => ((MainPageVistaModel)BindingContext);

        public MainPage()
        {
            InitializeComponent();
            buscarCancha.TextChanged += BuscarCancha_TextChanged;
        }

        protected override async void OnAppearing()
        {
            //var personList = await App.SQLiteDb.GetItemsAsync();
            //if (personList != null)
            //{
            //    VistaModelo.LoginViewModel.Usuarios.Add(new WSGOPLAY.Models.red.WoUsers { Username = personList[0].Name });
            //    contexto.IsLogueado = false;
            //    await Application.Current.MainPage.DisplayAlert("count", personList.Count.ToString(), "OK");
            //    //lstPersons.ItemsSource = personList;
            //}
            //else
            //{
            //    contexto. IsLogueado = true;
            //}
            ////base.OnAppearing();
        }

        private void BuscarCancha_TextChanged(object sender, TextChangedEventArgs e)
        {
            contexto.BuscarCanchaCommando.Execute(buscarCancha.Text);
        }

        private void SelectType(object sender, EventArgs e)
        {
            var view = sender as View;
            var parent = view.Parent as StackLayout;

            foreach (var child in parent.Children)
            {
                VisualStateManager.GoToState(child, "Normal");
                ChangeTextColor(child, "#707070");
            }

            VisualStateManager.GoToState(view, "Selected");
            ChangeTextColor(view, "#FFFFFF");

            var bindi32 = view.BindingContext as PropertyType;
            Application.Current.MainPage.DisplayAlert("", bindi32.Name, "OK");
        }

        private void ChangeTextColor(View child, string hexColor)
        {
            var txtCtrl = child.FindByName<Label>("PropertyTypeName");

            if (txtCtrl != null)
                txtCtrl.TextColor = Color.FromHex(hexColor);
        }

        private async void PropertySelected(object sender, EventArgs e)
        {
            var property = (sender as View).BindingContext as Property;
            await Application.Current.MainPage.Navigation.PushAsync(new DetailsPage(property));
            //await this.Navigation.PushAsync(new DetailsPage(property));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Contenido.BindingContext = contexto.PropertyList;
            //contexto.BindableLayout.ItemsSource
            //var se = new GoPlayServicio();
            //var x = await se.GetDatosAsync<CanchasModel>(Url.urlPages);
            //foreach (var item in x)
            //{
            //    await DisplayAlert("", item.PageTitle, "OK");
            //}
           
            await DisplayAlert("", contexto.PropertyList.Count.ToString(), "OK");
        }

        //private async void IniciarSesion_Clicked(object sender, EventArgs e)
        //{
         
        //}

        private async void CerrarSesion_Clicked(object sender, EventArgs e)
        {
            contexto.IsLogueadoNO = false;
            await App.SQLiteDb.DeleteItemAsync();
            contexto.IsLogueado = true;

        }
    }

    public class PropertyType
    {
        public string TypeName { get; set; }
        public string Name { get; set; }
    }

    public class Property
    {
        public string Id => Guid.NewGuid().ToString("N");
        public string PropertyName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Price { get; set; }
        public string Bed { get; set; }
        public string Bath { get; set; }
        public string Space { get; set; }
        public string Details { get; set; }
    }
}
