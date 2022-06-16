using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SystemPlus.Vectors;

namespace SystemPlus.ClassSupport
{
    public abstract class SaveSupportPlus<T> : ISaveSupport<T> where T : SaveSupportPlus<T>
    {
        protected abstract BindingFlags BindingFlagsProperties { get; }
        protected abstract BindingFlags BindingFlagsFields { get; }

        protected static BindingFlags BindingFlagsDefault { get; } = BindingFlags.Public | BindingFlags.Instance;

        public abstract T Default { get; }

        public virtual void LoadFromString(string data, bool exept = true)
        {
            Type overwrider = GetType();
            PropertyInfo[] propers = overwrider.GetProperties(BindingFlagsProperties);
            FieldInfo[] fields = overwrider.GetFields(BindingFlagsFields);

            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(this, fields[i].GetValue(Default));

            for (int i = 0; i < propers.Length; i++)
                try
                {
                    propers[i].SetValue(this, propers[i].GetValue(Default));
                } catch { }

            List<(string name, object value, Type type)> loaded = new List<(string name, object value, Type type)>();


            /*if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.", path);*/

            string[] values = /*File.ReadAllText(path)*/data.Replace(" ", string.Empty).Split(';');

            for (int i = 1; i < values.Length - 1; i++)
            {
                string[] split = values[i].Split(':');
                Type t = Type.GetType(split[0]);
                if (t == null)
                    continue;

                try
                {
                    if (t == typeof(byte))
                        loaded.Add((split[1], byte.Parse(split[2]), t));
                    else if (t == typeof(short))
                        loaded.Add((split[1], short.Parse(split[2]), t));
                    else if (t == typeof(int))
                        loaded.Add((split[1], int.Parse(split[2]), t));
                    else if (t == typeof(long))
                        loaded.Add((split[1], long.Parse(split[2]), t));
                    else if (t == typeof(float))
                        loaded.Add((split[1], float.Parse(split[2]), t));
                    else if (t == typeof(double))
                        loaded.Add((split[1], double.Parse(split[2]), t));
                    else if (t == typeof(bool))
                        loaded.Add((split[1], bool.Parse(split[2]), t));
                    else if (t == typeof(string))
                        loaded.Add((split[1], split[2], t));
                    else if (t == typeof(Vector2Int))
                        loaded.Add((split[1], Vector2Int.FromString(split[2]), t));
                    else
                        throw new Exception($"Type {t} not supported");
                }
                catch (Exception e)
                {
                    if (e.ToString() == $"Type {t} not supported")
                        throw e;
                    Console.WriteLine($"Field / Variable says is of type {t} but the value({split[2]}) is not.  e: " + e.ToString());
                }
            }


            for (int i = 0; i < loaded.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < propers.Length; j++)
                    if (propers[j].Name == loaded[i].name)
                        if (propers[j].PropertyType == loaded[i].type)
                        {
                            propers[j].SetValue(this, loaded[i].value);
                            found = true;
                            break;
                        }
                        else
                            throw new Exception($"Names match ({propers[j].Name},{loaded[i].name}) " +
                                $"but Types not ({propers[j].PropertyType},{loaded[i].type})");
                if (!found) // if stil not found search fields
                    for (int j = 0; j < fields.Length; j++)
                        if (fields[j].Name == loaded[i].name)
                            if (fields[j].FieldType == loaded[i].type)
                            {
                                fields[j].SetValue(this, loaded[i].value);
                                found = true;
                                break;
                            }
                            else
                                throw new Exception($"Names match ({propers[j].Name},{loaded[i].name}) " +
                                    $"but Types not ({propers[j].PropertyType},{loaded[i].type})");

                if (!found && exept)
                    throw new Exception($"Propery / Field {loaded[i].name} of type {loaded[i].type} was not " +
                        $"found in {overwrider} type.");
            }
        }

        public virtual void Load(string path, bool exept = true)
        {
            Type overwrider = GetType();
            PropertyInfo[] propers = overwrider.GetProperties(BindingFlagsProperties);
            FieldInfo[] fields = overwrider.GetFields(BindingFlagsFields);

            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(this, fields[i].GetValue(Default));

            for (int i = 0; i < propers.Length; i++)
                try
                {
                    propers[i].SetValue(this, propers[i].GetValue(Default));
                }
                catch { }

            List<(string name, object value, Type type)> loaded = new List<(string name, object value, Type type)>();


            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist.", path);

            string[] values = File.ReadAllText(path).Replace(" ", string.Empty).Split(';');

            for (int i = 1; i < values.Length - 1; i++)
            {
                string[] split = values[i].Split(':');
                Type t = Type.GetType(split[0]);
                if (t == null)
                    continue;

                try
                {
                    if (t == typeof(byte))
                        loaded.Add((split[1], byte.Parse(split[2]), t));
                    else if (t == typeof(short))
                        loaded.Add((split[1], short.Parse(split[2]), t));
                    else if (t == typeof(int))
                        loaded.Add((split[1], int.Parse(split[2]), t));
                    else if (t == typeof(long))
                        loaded.Add((split[1], long.Parse(split[2]), t));
                    else if (t == typeof(float))
                        loaded.Add((split[1], float.Parse(split[2]), t));
                    else if (t == typeof(double))
                        loaded.Add((split[1], double.Parse(split[2]), t));
                    else if (t == typeof(bool))
                        loaded.Add((split[1], bool.Parse(split[2]), t));
                    else if (t == typeof(string))
                        loaded.Add((split[1], split[2], t));
                    else if (t == typeof(Vector2Int))
                        loaded.Add((split[1], Vector2Int.FromString(split[2]), t));
                    else
                        throw new Exception($"Type {t} not supported");
                }
                catch (Exception e)
                {
                    if (e.ToString() == $"Type {t} not supported")
                        throw e;
                    Console.WriteLine($"Field / Variable says is of type {t} but the value({split[2]}) is not.  e: " + e.ToString());
                }
            }


            for (int i = 0; i < loaded.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < propers.Length; j++)
                    if (propers[j].Name == loaded[i].name)
                        if (propers[j].PropertyType == loaded[i].type)
                        {
                            propers[j].SetValue(this, loaded[i].value);
                            found = true;
                            break;
                        }
                        else
                            throw new Exception($"Names match ({propers[j].Name},{loaded[i].name}) " +
                                $"but Types not ({propers[j].PropertyType},{loaded[i].type})");
                if (!found) // if stil not found search fields
                    for (int j = 0; j < fields.Length; j++)
                        if (fields[j].Name == loaded[i].name)
                            if (fields[j].FieldType == loaded[i].type)
                            {
                                fields[j].SetValue(this, loaded[i].value);
                                found = true;
                                break;
                            }
                            else
                                throw new Exception($"Names match ({propers[j].Name},{loaded[i].name}) " +
                                    $"but Types not ({propers[j].PropertyType},{loaded[i].type})");

                if (!found && exept)
                    throw new Exception($"Propery / Field {loaded[i].name} of type {loaded[i].type} was not " +
                        $"found in {overwrider} type.");
            }
        }

        public virtual string SaveToString()
        {
            Type overwrider = GetType();
            PropertyInfo[] propers = overwrider.GetProperties(BindingFlagsProperties);
            FieldInfo[] fields = overwrider.GetFields(BindingFlagsFields);

            string text = "{;\n";

            for (int i = 0; i < propers.Length; i++)
            {
                Type t = propers[i].PropertyType;

                if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long)
                    || t == typeof(float) || t == typeof(double) || t == typeof(string))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(bool))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(Vector2Int))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
            }
            for (int i = 0; i < fields.Length; i++)
            {
                Type t = fields[i].FieldType;

                if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long)
                    || t == typeof(float) || t == typeof(double) || t == typeof(string))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(bool))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(Vector2Int))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
            }
            text += "}";
            return text;//File.WriteAllText(path, text);
        }

        public virtual void Save(string path)
        {
            Type overwrider = GetType();
            PropertyInfo[] propers = overwrider.GetProperties(BindingFlagsProperties);
            FieldInfo[] fields = overwrider.GetFields(BindingFlagsFields);

            string text = "{;\n";

            for (int i = 0; i < propers.Length; i++)
            {
                Type t = propers[i].PropertyType;

                if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long)
                    || t == typeof(float) || t == typeof(double) || t == typeof(string))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(bool))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(Vector2Int))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
            }
            for (int i = 0; i < fields.Length; i++)
            {
                Type t = fields[i].FieldType;

                if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long)
                    || t == typeof(float) || t == typeof(double) || t == typeof(string))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(bool))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
                else if (t == typeof(Vector2Int))
                    text += t.ToString() + ":" + propers[i].Name + ":" + propers[i].GetValue(this).ToString() + ";\n";
            }
            text += "}";
            File.WriteAllText(path, text);
        }
    }
}
