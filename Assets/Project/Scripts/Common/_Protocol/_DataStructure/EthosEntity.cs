using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class EthosEntity
    {
        [SerializeField] private EthosType ethosType;
        [SerializeField] private string ethosKey;
        [SerializeField] private int value;
            
        public EthosType EthosType => ethosType;
        public string EthosKey => ethosKey;

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(this);
            }
        }

        public Action<EthosEntity> OnValueChanged { get; set; }
        
        public EthosEntity(EthosType ethosType, string ethosKey, int value)
        {
            this.ethosType = ethosType;
            this.ethosKey  = ethosKey;
            this.value     = value;
        }

        public void SetEthos(EthosType ethosType, int value)
        {
            this.ethosType = ethosType;
            Value          = value;
        }
    }
}


