﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
    /// <summary>
    /// Podmínka, která neguje vnitřní (kompozitní) podmínky.
    /// </summary>
    [Serializable]
    public class NotCondition : Condition
    {
        #region Conditions
        /// <summary>
        /// Podmínky, které jsou negovány. Mezi podmínkami je operátor AND.
        /// </summary>
        public ConditionList Conditions
        {
            get
            {
                return _andConditions.Conditions;
            }
        }
        private AndCondition _andConditions; 
        #endregion

        #region Constructors
        /// <summary>
        /// Vytvoří instanci podmínky NotCondition a případně ji inicializuje zadanými vnitřními podmínkami.
        /// </summary>
        public NotCondition(params Condition[] conditions)
        {
            _andConditions = new AndCondition();
            if (conditions != null)
            {
                for (int index = 0; index < conditions.Length; index++)
                {
                    _andConditions.Conditions.Add(conditions[index]);
                }
            }
        }
        #endregion

        #region GetWhereStatement
        /// <summary>
        /// Přidá část SQL příkaz pro sekci WHERE.
        /// </summary>
        public override void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
        {
            if (IsEmptyCondition())
            {
                return;
            }

            whereBuilder.Append("NOT ("); 
            _andConditions.GetWhereStatement(command, whereBuilder);
            whereBuilder.Append(")");
        } 
        #endregion

        #region IsEmptyCondition
        /// <summary>
        /// Udává, zda podmínka reprezentuje prázdnou podmínku, která nebude renderována.
        /// </summary>
        public override bool IsEmptyCondition()
        {
            return _andConditions.IsEmptyCondition();
        }
        #endregion

        #region Create (static)
        /// <summary>
        /// Vytvoří instanci podmínky NotCondition a případně ji inicializuje zadanými vnitřními podmínkami.
        /// </summary>
        public static NotCondition Create(params Condition[] conditions)
        {
            return new NotCondition(conditions);
        } 
        #endregion
    }
}
