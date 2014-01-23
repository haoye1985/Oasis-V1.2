using System;

namespace Oasis_v1._1
{
    public enum NetworkType
    {
        /// <summary>
        /// The Default network is empty network typle and need to be filled with other network type
        /// </summary>
        Default_Network,
        /// <summary>
        /// The Road network is the stardard network representing any road network in Oasis to implement transport network analysis
        /// <para>This is the standard way</para>
        /// </summary>
        Road_NetWork,

        /// <summary>
        /// The Railway network is the stardard network representing any rail network in Oasis to implement transport network analysis
        /// <para>This is the standard way</para>
        /// </summary>
        Railway_Network,

        /// <summary>
        /// The Tube network is the stardard network representing any Tube network in Oasis to implement transport network analysis
        /// <para>This is the standard way</para>
        /// </summary>
        Tube_Network,

        /// <summary>
        /// The Other network is the stardard network representing any others network in Oasis to implement transport network analysis
        /// <para>This is the standard way</para>
        /// </summary>
        Other_Network
    }
}
