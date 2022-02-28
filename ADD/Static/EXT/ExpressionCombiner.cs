﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace cpADD.EXT
{
    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> AND<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> second_Exp)
        {
            // get the visitor
            var visitor = new ParameterUpdateVisitor(second_Exp.Parameters.First(), exp.Parameters.First());
            // replace the parameter in the expression just created
            second_Exp = visitor.Visit(second_Exp) as Expression<Func<T, bool>>;

            // now you can and together the two expressions
            var binExp = Expression.And(exp.Body, second_Exp.Body);
            // and return a new lambda, that will do what you want. NOTE that the binExp has reference only to te newExp.Parameters[0] (there is only 1) parameter, and no other
            return Expression.Lambda<Func<T, bool>>(binExp, second_Exp.Parameters);
        }

        public static Expression<Func<T, bool>> OR<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> second_Exp)
        {
            // get the visitor
            var visitor = new ParameterUpdateVisitor(second_Exp.Parameters.First(), exp.Parameters.First());
            // replace the parameter in the expression just created
            second_Exp = visitor.Visit(second_Exp) as Expression<Func<T, bool>>;

            // now you can and together the two expressions
            var binExp = Expression.Or(exp.Body, second_Exp.Body);
            // and return a new lambda, that will do what you want. NOTE that the binExp has reference only to te newExp.Parameters[0] (there is only 1) parameter, and no other
            return Expression.Lambda<Func<T, bool>>(binExp, second_Exp.Parameters);
        }



        class ParameterUpdateVisitor : ExpressionVisitor
        {
            private ParameterExpression _oldParameter;
            private ParameterExpression _newParameter;

            public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (object.ReferenceEquals(node, _oldParameter))
                    return _newParameter;

                return base.VisitParameter(node);
            }
        }
    }



}
