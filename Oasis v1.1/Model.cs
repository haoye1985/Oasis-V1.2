using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pan.Utilities;

namespace Oasis_v1._1
{
    public static class Model
    {
        public static void BuildStaticModel()
        {
            foreach (var _infraNode in States.Network.InfraNodes)
            {
                _infraNode.RiskXi = 0;
            }

            List<InfraNode> Node = States.Network.InfraNodes;
            List<InfraLink> Link = States.Network.InfraLinks;

            float[ , ] A = new float[Node.Count, Node.Count];
            float[ ] C = new float[Node.Count];

            Messages.ClearMessages();

            // Populate Aij
            for (int i = 0; i < Node.Count; i++)
            {
                for (int j = 0; j < Node.Count; j++)
                {
                    foreach (InfraLink ij in Link)
                    {
                        if (ij.FromNode == Node[i] && ij.ToNode == Node[j])
                        {
                            A[i, j] = ij.RiskAij;
                        }
                    }
                }
            }


            // Populate Ci
            for (int i = 0; i < Node.Count; i++)
            {
                C[i] = Node[i].RiskCi;
            }

            Messages.Print("\n LEONTIEF MODEL");

            for (int i = 0; i < Node.Count; i++)
            {
                string x_ij = "";
                for (int j = 0; j < Node.Count; j++)
                {
                    if (i != j)
                    {
                        x_ij += string.Format("x{1} * a{1}{0} + ", i, j);

                    }
                   
                }
            }

            Messages.Print("");
            Messages.Print("LEONTIEF MODEL VALUES");


        }
    }
}
