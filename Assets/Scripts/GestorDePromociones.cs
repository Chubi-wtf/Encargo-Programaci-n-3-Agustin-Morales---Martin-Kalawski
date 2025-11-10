using UnityEngine;
using System.Collections.Generic;

public static class GestorDePromociones
{
    #region Estructuras y Variables
    public struct ResultadoPromocion
    {
        public float totalDescuentoAplicado;
    }

    private static readonly Dictionary<string, float> promocionesActivas = new Dictionary<string, float>()
    {
        { "verdura", 0.50f },
        { "panaderia", 0.20f },
    };
    #endregion

    #region Lógica de Descuentos
    public static (float subtotal, float descuentoTotal) AplicarDescuentos(List<ItemInstance> itemsAComprar)
    {
        float subtotal = 0f;
        float descuentoTotal = 0f;

        foreach (ItemInstance item in itemsAComprar)
        {
            float precioOriginal = item.precioDeEsteItem;
            float descuentoItem = 0f;

            string categoria = item.datosDelItem.itemCategory.ToLower().Trim();

            if (promocionesActivas.ContainsKey(categoria))
            {
                float porcentajeDesc = promocionesActivas[categoria];

                descuentoItem = precioOriginal * porcentajeDesc;
                descuentoTotal += descuentoItem;
            }

            subtotal += precioOriginal;
        }

        subtotal = Mathf.Round(subtotal * 100f) / 100f;
        descuentoTotal = Mathf.Round(descuentoTotal * 100f) / 100f;

        return (subtotal, descuentoTotal);
    }
    #endregion
}