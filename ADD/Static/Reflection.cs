using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace cpADD.Static
{
    public static class czReflection
    {
        public delegate object ObjectActivator(params object[] args);



        public static object cfCreate_Object<A>(Type cxObject_Type, Func<Type, string, object> cxGetObject_from_Doc) where A : Attribute
        {
            var cxCtor = czReflection.cfGet_Ctor_by_Attribute<A>(cxObject_Type);
            if (cxCtor == null) throw new Exception("There is no ctor function!");
            return czReflection.cfCreate_Obj(cxCtor, cfGet_Params(cxCtor, cxGetObject_from_Doc));

            object[] cfGet_Params(ConstructorInfo cxCtor,Func<Type, string, object> cxGetObject)
            {
                List<object> cxParams = new List<object>();
                foreach (var p in cxCtor.GetParameters()) cxParams.Add(cxGetObject(p.ParameterType, p.Name));
                return cxParams.ToArray();
            }
        }


        //public static object cfCreate_Object<A,D>(Type cxObject_Type, D cxDoc, Func<D,Type,string,object> cxGetObject_by_Doc) where A : Attribute
        //{
        //    var cxCtor = czReflection.cfGet_Ctor_by_Attribute<A>(cxObject_Type);
        //    if (cxCtor == null) throw new Exception("There is no ctor function!");
        //    return czReflection.cfCreate_Obj(cxCtor, cfGet_Params(cxCtor, cxDoc, cxGetObject_by_Doc));

        //    object[] cfGet_Params(ConstructorInfo cxCtor, D cxDoc, Func<D, Type, string, object> cxGetObject)
        //    {
        //        List<object> cxParams = new List<object>();
        //        foreach (var p in cxCtor.GetParameters()) cxParams.Add(cxGetObject(cxDoc, p.ParameterType, p.Name));
        //        return cxParams.ToArray();
        //    }
        //}




        public static ConstructorInfo cfGet_Ctor_by_Attribute<T>(Type cxType) where T: Attribute
        {
            var cxCtors = cxType.GetConstructors().Where(x => x.GetCustomAttribute<T>() != null);
            //var cxMembers = cfGetTypeMembers(cxInterface_type).Select(s => s.Name.ToLower());
            //var cxctor = cxctors.FirstOrDefault(x => x.GetParameters().All(p => cxMembers.Any(m => m == p.Name.ToLower())));

            if (cxCtors.Count() == 0) throw new Exception("There is no ctor function!");
            if (cxCtors.Count() > 1) throw new Exception("There is many ctor function!");

            return cxCtors.First();
        }

        public static IEnumerable<MemberInfo> cfGet_Members_by_Type(Type cxType)
        {
            var cxMembers = new List<MemberInfo>();
            var flags = (BindingFlags.Public | BindingFlags.Instance);
            cxMembers.AddRange(cxType.GetProperties(flags).Where(x => x.CanRead && x.GetIndexParameters().Length == 0).Select(x => x as MemberInfo));
            return cxMembers;
        }

        public static object cfCreate_Obj(ConstructorInfo cxCtor, object[] cxParams)
        {
            var delegCreate = cfGetActivator(cxCtor);
            return delegCreate(cxParams);
        }

        public static ObjectActivator cfGetActivator(ConstructorInfo cxCtor)
        {
            Type type = cxCtor.DeclaringType;
            ParameterInfo[] paramsInfo = cxCtor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(cxCtor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }






    }

}





//object cfCreate_Object(Type cxObject_Type, BsonDocument cxDoc)
//{
//    var cxCtor = czReflection.cfGet_Ctor_by_Attribute<BsonCtorAttribute>(cxObject_Type);
//    if (cxCtor == null) throw new Exception("There is no ctor function!");
//    return czReflection.cfCreate_Obj(cxCtor, cfGet_Params(cxCtor, cxDoc));

//    object[] cfGet_Params(ConstructorInfo cxCtor, BsonDocument cxDoc)
//    {
//        List<object> cxParams = new List<object>();
//        foreach (var p in cxCtor.GetParameters())
//        {
//            if (cxDoc.TryGetValue(p.Name, out var cxDoc_Value))
//            {
//                var arg = this.Deserialize(p.ParameterType, cxDoc_Value);
//                cxParams.Add(arg);
//            }
//            else throw new Exception($"There is no {p.Name} in Doc!");
//        }
//        return cxParams.ToArray();
//    }
//}