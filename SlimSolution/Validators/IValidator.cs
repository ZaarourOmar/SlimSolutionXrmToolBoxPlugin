using Microsoft.Xrm.Sdk;
using SlimSolution.Models;
using System;
using System.Threading.Tasks;

namespace SlimSolution.Validators
{
    public interface IValidator
    {
        event EventHandler<ErrorEventArgs> OnValidatorError;
        event EventHandler<ProgressEventArgs> OnValidatorProgress;

        IOrganizationService CRMService { get; set; }
        string Message { get; }

        ValidationResults Validate(CRMSolution solution);
    }

    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
        }
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; set; }
        public String Message { get; set; }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string message, int progress)
        {
            Message = message;
            Progress = progress;
        }

        public ProgressEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public int Progress { get; set; }
    }
}

