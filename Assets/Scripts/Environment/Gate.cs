using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Booster
{
    public enum GateType { LikeGate, DislikeGate }
    public enum LikeBoosters { Plus10, Plus20, Plus30, Plus40, x1, x2, x3 }
    public enum DislikeBoosters { Minus10, Minus20, Minus30, d1, d2 }

    public GateType gateType;
    public LikeBoosters likeBooster;
    public DislikeBoosters dislikeBooster;
}

public class Gate : MonoBehaviour
{
    [SerializeField] private Text _boostText;
    [SerializeField] private GameObject _field;

    public Booster booster;

    private void Awake()
    {                
        switch (booster.gateType)
        {
            case Booster.GateType.LikeGate:
                switch (booster.likeBooster)
                {
                    case Booster.LikeBoosters.Plus10:
                        _boostText.text = "+10";
                        break;
                    case Booster.LikeBoosters.Plus20:
                        _boostText.text = "+20";
                        break;
                    case Booster.LikeBoosters.Plus30:
                        _boostText.text = "+30";
                        break;
                    case Booster.LikeBoosters.Plus40:
                        _boostText.text = "+40";
                        break;
                    case Booster.LikeBoosters.x1:
                        _boostText.text = "x1";
                        break;
                    case Booster.LikeBoosters.x2:
                        _boostText.text = "x2";
                        break;
                    case Booster.LikeBoosters.x3:
                        _boostText.text = "x3";
                        break;
                    default:
                        break;
                }
                break;
            case Booster.GateType.DislikeGate:
                switch (booster.dislikeBooster)
                {
                    case Booster.DislikeBoosters.Minus10:
                        _boostText.text = "-10";
                        break;
                    case Booster.DislikeBoosters.Minus20:
                        _boostText.text = "-20";
                        break;
                    case Booster.DislikeBoosters.Minus30:
                        _boostText.text = "-30";
                        break;
                    case Booster.DislikeBoosters.d1:
                        _boostText.text = "/1";
                        break;
                    case Booster.DislikeBoosters.d2:
                        _boostText.text = "/2";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }

    public void RemoveField() 
    {
        Destroy(_field);
        Destroy(_boostText.gameObject);
    }
}
