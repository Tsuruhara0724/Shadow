using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public sealed class ObjectKvp : UnityNameValuePair<Object>
{
    public Object value = null; // 值对象

    override public Object Value
    {
        get { return this.value; } // 获取值对象
        set { this.value = value; } // 设置值对象
    }

    // 构造函数，初始化键值对
    public ObjectKvp(string key, Object value) : base(key, value)
    {
    }
}

[System.Serializable]
public class ObjectDictionary : UnityDictionary<Object>
{
    public List<ObjectKvp> values; // 存储键值对的列表

    // 获取和设置键值对列表
    override protected List<UnityKeyValuePair<string, Object>> KeyValuePairs
    {
        get
        {
            return values.ConvertAll<UnityKeyValuePair<string, Object>>(new System.Converter<ObjectKvp, UnityKeyValuePair<string, Object>>(
            x => {
                return x as UnityKeyValuePair<string, Object>;
            }));
        }
        set
        {
            if (value == null)
            {
                values = new List<ObjectKvp>(); // 初始化列表
                return;
            }

            // 将键值对列表转换为 ObjectKvp 类型的列表
            values = value.ConvertAll<ObjectKvp>(new System.Converter<UnityKeyValuePair<string, Object>, ObjectKvp>(
            x => {
                return new ObjectKvp(x.Key, x.Value as Object); // 转换键值对类型
            }));
        }
    }

    // 设置键值对的方法
    override protected void SetKeyValuePair(string k, Object v)
    {
        var index = values.FindIndex(x => {
            return x.Key == k;
        }); // 查找键是否存在

        if (index != -1)
        {
            if (v == null)
            {
                values.RemoveAt(index); // 删除键值对
                return;
            }

            values[index] = new ObjectKvp(k, v); // 更新键值对
            return;
        }

        values.Add(new ObjectKvp(k, v)); // 添加新的键值对
    }
}
