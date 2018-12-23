using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfCalculationLib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface ICalcService {

        [OperationContract]
        OutputDate CulcTeploParal(InputDate inputMatrixes);

        [OperationContract]
        OutputDate CulcTeploPosl(InputDate inputMatrixes);

    }
    [DataContract]
    public class InputDate {
        double[][] mass_u;
        double time;
        double h;
        double tau;

        [DataMember]
        public double[][] Mass_u {
            get { return mass_u; }
            set { mass_u = value; }
        }

        [DataMember]
        public double Time {
            get { return time; }
            set { time = value; }
        }

        [DataMember]
        public double H {
            get { return h; }
            set { h = value; }
        }

        [DataMember]
        public double Tau {
            get { return tau; }
            set { tau = value; }
        }

    }

    [DataContract]
    public class OutputDate {
        double[][] culc;

        [DataMember]
        public double[][] Culc_Teplo {
            get { return culc; }
            set { culc = value; }
        }
    }
}
