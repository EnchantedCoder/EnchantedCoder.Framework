using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
    /// <summary>
    /// Podm�nka, kter� neguje vnit�n� (kompozitn�) podm�nky.
    /// </summary>
    [Serializable]
    public class NotCondition : Condition
    {
        #region Conditions
        /// <summary>
        /// Podm�nky, kter� jsou negov�ny. Mezi podm�nkami je oper�tor AND.
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
        /// Vytvo�� instanci podm�nky NotCondition a p��padn� ji inicializuje zadan�mi vnit�n�mi podm�nkami.
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
        /// P�id� ��st SQL p��kaz pro sekci WHERE.
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
        /// Ud�v�, zda podm�nka reprezentuje pr�zdnou podm�nku, kter� nebude renderov�na.
        /// </summary>
        public override bool IsEmptyCondition()
        {
            return _andConditions.IsEmptyCondition();
        }
        #endregion

        #region Create (static)
        /// <summary>
        /// Vytvo�� instanci podm�nky NotCondition a p��padn� ji inicializuje zadan�mi vnit�n�mi podm�nkami.
        /// </summary>
        public static NotCondition Create(params Condition[] conditions)
        {
            return new NotCondition(conditions);
        } 
        #endregion
    }
}
