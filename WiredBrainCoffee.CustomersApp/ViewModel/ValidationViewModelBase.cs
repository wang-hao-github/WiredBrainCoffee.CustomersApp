using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WiredBrainCoffee.CustomersApp.ViewModel
{
    public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null && _errorsByPropertyName.ContainsKey(propertyName)
                ? _errorsByPropertyName[propertyName]
                : Enumerable.Empty<string>();
        }
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
        protected void AddError(string error, [CallerMemberName]string? propertyName=null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Add(propertyName, new List<string>());
            }
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
        protected void ClearErrors([CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                return;
            }
            _errorsByPropertyName.Remove(propertyName);
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }
    }
}
