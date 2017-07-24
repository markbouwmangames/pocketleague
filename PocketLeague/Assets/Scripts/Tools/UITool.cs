using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool : MonoBehaviour {

    public static T CreateField<T>(T original) where T : MonoBehaviour {
        return CreateField<T>(original.gameObject);
    }

    public static T CreateField<T>(GameObject original) {
        var newGO = GameObject.Instantiate(original);

        newGO.transform.SetParent(original.transform.parent);
        newGO.transform.localScale = original.transform.localScale;

        newGO.SetActive(true);
        return newGO.GetComponent<T>();
    }
}
