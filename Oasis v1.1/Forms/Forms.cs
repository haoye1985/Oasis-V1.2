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
        public static fmConsole dockConsole;
        public static fmDesignDiagram dockDesignDiagram;
        public static fmNetwork dockNetwork;

        public static void Initialise()
        {
            dockMain = new MainWin();
            dockManager.Initialize(dockMain.dockPanel1);

            var _DockContents = new DockContent[]
            {
                dockMap = new fmMap(),
                dockConsole = new fmConsole(),
                dockDesignDiagram = new fmDesignDiagram(),
                dockNetwork = new fmNetwork(),
                dockNode = new fmNode(),
                dockLink = new fmLink(),
            };
            dockManager.AddDocks(_DockContents);

            //Here you have to activiate the programs at Program file
            dockMain.Initialize();
            
        }


    }
}
