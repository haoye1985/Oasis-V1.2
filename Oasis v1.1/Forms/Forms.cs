using WeifenLuo.WinFormsUI.Docking;
using Pan.Docks;

namespace Oasis_v1._1
{
    public static class Forms
    {
        public static DockManager dockManager = new DockManager();
        public static MainWin dockMain;
        public static fmMap dockMap;
        public static fmNode dockNode;
        public static fmLink dockLink;
        public static fmDiagram dockDiagram;
        public static fmConsole dockConsole;
        public static fmDesignDiagram dockDesignDiagram;

        public static void Initialise()
        {
            dockMain = new MainWin();
            dockManager.Initialize(dockMain.dockPanel1);

            var _DockContents = new DockContent[]
            {
                dockNode = new fmNode(),
                dockLink = new fmLink(),
                dockDiagram = new fmDiagram(),
                dockConsole = new fmConsole(),
                dockDesignDiagram = new fmDesignDiagram(),
                dockMap = new fmMap(),
            };

            dockManager.AddDocks(_DockContents);

            //Here you have to activiate the programs at Program file
            dockMain.Initialize();
            
        }


    }
}
