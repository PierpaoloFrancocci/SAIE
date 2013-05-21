using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using BABollettari.Web.Services ;
using Telerik.Windows.Controls;



using Telerik.Windows;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using Telerik.Windows.Input;


using BABollettari.Events;

using BABollettari.Web.Services;
using BABollettari.Web.Models;


using System.ServiceModel.DomainServices.Client;


namespace BABollettari.Views.Bollettari
{
    public partial class MasterGrid : UserControl

    {

        public event EventHandler<ParentArgs> ParentRequest; 

        public MasterGrid()
        {
            if (BABollettari.Globals._context == null)
                BABollettari.Globals._context = new BollettarioDomainContext();

            InitializeComponent();
             
            
        }

        private void BollettarioDataGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (BollettarioDataGrid.SelectedItems.Count > 0)
            {
                v_Bollettario selectedBoll = (v_Bollettario)(this.BollettarioDataGrid.SelectedItems[0]);

                Dictionary<String, Object> parameters = new Dictionary<string, object>();

                parameters.Add("SelezioneGrid", "IdBollettario");
                parameters.Add("IdBollettario", selectedBoll.IdBollettario.ToString());
                parameters.Add("StatoBollettario", selectedBoll.StatoBollettario.ToString());
                
                if (ParentRequest != null)
                    ParentRequest(sender, new ParentArgs("GeneraBollettario", parameters));



            }

        }

        private void EmettiBollettario_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataPagerBollettario_PageIndexChanged(object sender, EventArgs e)
        {

        }

        private void BollettarioDetailsControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        public void Search(object sender, ExtListSearchArgs e)
        {
            string ProgTipografo= string.Empty ;

            if (BollettarioDataSource.CanLoad)
            {
                if (BollettarioDataSource.DomainContext == null)
                    BollettarioDataSource.DomainContext = BABollettari.Globals._context ;

                BollettarioDataSource.QueryParameters.Clear();

                if (e.Parameters.ContainsKey("ProgTipografo"))
                    if (e.Parameters["ProgTipografo"].ToString() != "*")
                        ProgTipografo = e.Parameters["ProgTipografo"].ToString();

                BollettarioDataSource.QueryParameters.Add(new Parameter { ParameterName = "progTipografo", Value = ProgTipografo.ToString() });
                BollettarioDataSource.Load();
            }
            else
                MessageBox.Show("Sto caricando i dati dal database. Un momento di pazienza", "", MessageBoxButton.OK);
        }

        public void PopolaGrid(string ProgTipografo )
        {
            if (BollettarioDataSource.DomainContext == null)
                BollettarioDataSource.DomainContext = BABollettari.Globals._context;

            BollettarioDataSource.QueryParameters.Clear();

            BollettarioDataSource.QueryParameters.Add(new Parameter { ParameterName = "progTipografo", Value = ProgTipografo.ToString () });
            BollettarioDataSource.Load();

        }

    }
}
