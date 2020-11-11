using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Report_ClassLibrary
{
    public class TemplateGenerator : ITemplate // Class inheriting ITemplate
    {
        ListItemType type;
        string columnName;
        string ctrlID;
        string ctrlType;
        string css;

        public TemplateGenerator(ListItemType t, string cN, string controlName, string ctrlTypeP, string cssP = "")
        {
            type = t;
            columnName = cN;
            ctrlID = controlName;
            ctrlType = ctrlTypeP;
            css = cssP;
        }

        // Override InstantiateIn() method
        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (ctrlType)
            {
                case "ddl":

                    DropDownList ddl = new DropDownList();
                    ddl.ID = ctrlID;

                    //ddl.DataBinding += new EventHandler(ddl_DataBinding);
                    container.Controls.Add(ddl);
                    break;

                case "btn":

                    Button btn = new Button();
                    btn.ID = ctrlID;
           
                    //ddl.DataBinding += new EventHandler(ddl_DataBinding);
                    container.Controls.Add(btn);
                    break;
            }
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
        }

        // The DataBinding event of your controls
        void ddl_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            //GridViewRow container = (GridViewRow)ddl.NamingContainer;
            //object bindValue = DataBinder.Eval(container.DataItem, columnName);
            //// Adding check in case Column allows null values
            //if (bindValue != DBNull.Value)
            {
                //ddl.BindDropdownList(routeAmtList);
            }
        }
    }
}
