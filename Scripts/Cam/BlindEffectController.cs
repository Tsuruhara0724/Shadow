using UnityEngine;

public class BlinkEffectController : MonoBehaviour
{
    public Shader blinkShader; // 眨眼特效的Shader
    private Material blinkMaterial; // 眨眼特效的材质

    void Start()
    {
        // 创建眨眼特效的材质并赋予Shader
        blinkMaterial = new Material(blinkShader);

        // 将眨眼特效的材质赋予空的GameObject的渲染器
        GetComponent<Renderer>().material = blinkMaterial;
    }

    // 触发眨眼特效的方法
    public void TriggerBlinkEffect()
    {
        // 修改Shader属性，例如修改眨眼特效的参数
        blinkMaterial.SetVector("_Param", new Vector4(0.5f, 0.5f, 1.0f, 1.0f));
    }
}