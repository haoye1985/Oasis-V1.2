﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Dataweb.NShape;
using Dataweb.NShape.GeneralShapes;

namespace Oasis_v1._1
{
    public partial class fmDiagram : DockContent
    {
        private Diagram diagram;

        public fmDiagram()
        {
            InitializeComponent();
        }
    }
}
