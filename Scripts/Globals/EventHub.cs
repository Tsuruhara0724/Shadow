using UnityEngine;
using System.Collections;

public class EventHub : MonoBehaviour
{

    #region Event delegates
    public delegate void VoidEvent(); // 无参数事件委托
    public delegate void IntegerParamEvent(int value); // 整数参数事件委托
    public delegate void GameObjectParamEvent(GameObject obj); // 游戏对象参数事件委托
    public delegate void GameObjectIntegerParamEvent(GameObject enemy, int value); // 游戏对象和整数参数事件委托
    #endregion

    #region Events
    public event VoidEvent ExampleVoidEvent; // 示例无参数事件
    public event IntegerParamEvent ExampleIntegerEvent; // 示例整数参数事件
    public event GameObjectParamEvent ExampleGameObjectEvent; // 示例游戏对象参数事件
    public event GameObjectIntegerParamEvent ExampleCombinedEvent; // 示例游戏对象和整数参数事件
    #endregion

    #region Triggers
    public void TriggerExampleIntegerEvent(int val)
    {
        ExampleIntegerEvent?.Invoke(val); // 触发示例整数参数事件
    }

    public void TriggerExampleVoidEvent()
    {
        ExampleVoidEvent?.Invoke(); // 触发示例无参数事件
    }

    #endregion
}