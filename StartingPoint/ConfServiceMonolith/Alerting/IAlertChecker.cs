using System;
using System.Threading;

namespace Alerting
{
    public interface IAlertChecker
    {
        event Action<TemperatureTooLowEventArgs> OnTemperatureTooLowAlert;
        void CheckAndRaiseAlertIfNeeded(CancellationToken cancellationToken);
    }
}