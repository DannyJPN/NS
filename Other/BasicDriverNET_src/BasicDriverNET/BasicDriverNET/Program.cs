using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDriverNET
{
    class Program
    {
        static void Main(string[] args)
        {
		    String host = "localhost";
		    int port = 9461;
		    String raceName = "test";
		    String driverName = "basic_client";
		    String carType = null;
		    RaceConnector raceConnector = null;
		    if (args.Length < 4) {
			    // kontrola argumentu programu
			    raceConnector = new RaceConnector(host, port, null);
			    Console.WriteLine("argumenty: server port nazev_zavodu jmeno_ridice [typ_auta]");
			    List<String> raceList =  raceConnector.listRaces();
			    raceName = raceList[new Random().Next(raceList.Count ) ];
			    List<String> carList =  raceConnector.listCars(raceName);
			    carType = carList[new Random().Next(carList.Count)];
			    driverName += "_" + carType;
    //			host = JOptionPane.showInputDialog("Host:", host);
    //			port = Integer.parseInt(JOptionPane.showInputDialog("Port:", Integer.toString(port)));
    //			raceName = JOptionPane.showInputDialog("Race name:", raceName);
    //			driverName = JOptionPane.showInputDialog("Driver name:", driverName);
		    } else {
			    // nacteni parametu
			    host = args[0];
			    port = int.Parse(args[1]);
			    raceName = args[2];
			    driverName = args[3];
			    if(args.Length > 4){
				    carType = args[4];
			    }
			    raceConnector = new RaceConnector(host, port, null);
		    }
		    // vytvoreni klienta
		    raceConnector.setDriver(new SimpleDriver());
		    raceConnector.start(raceName, driverName, carType);
	    }
    }
}
