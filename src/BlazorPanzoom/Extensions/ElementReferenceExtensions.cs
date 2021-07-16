using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public static class ElementReferenceExtensions
    {
        private static ElementReference DefaultElementReference => default;

        public static bool IsDefault(this ElementReference elementReference) =>
            elementReference.Equals(DefaultElementReference);
    }
}