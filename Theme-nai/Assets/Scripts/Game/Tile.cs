using System;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    private SpriteRenderer spRen;
    [SerializeField] private Color32 PHASE_0;
    [SerializeField] private Color32 PHASE_1;
    [SerializeField] private Color32 PHASE_2;
    [SerializeField] private Color32 PHASE_3;
    [SerializeField] private Color32 PHASE_4;
    [SerializeField] private Animator anim;
    [SerializeField] private Id id;
    public int phase = 0;
    public int maxPhase = 1;
    private bool canClick;

    public Action<Id> OnClickTile; // タイルクリック時のイベント
    
    private int Mod(int a, int b)
    {
        return ((a % b) + b) % b;
    }

    public void SetIndex(int x, int y)
    {
        id = new Id(x, y);
    }

    public Id GetId()
    {
        if (id.isNull)
        {
            Debug.LogError("id is NULL!");
            return Id.zero;
        }
        
        return id;
    }

    public void SetPhase(int phase)
    {
        this.phase = phase;
    }

    public int GetPhase()
    {
        return Mod(phase, maxPhase+1);
    }

    public void SetMaxPhase(int maxPhase)
    {
        this.maxPhase = maxPhase;
    }

    public void ChangeColor()
    {
        if (spRen == null)
        {
            spRen = GetComponent<SpriteRenderer>();
        }

        switch(Mod(phase, maxPhase+1))
        {
        case 0:
            spRen.material.color = PHASE_0;
            break;
        case 1:
            spRen.material.color = PHASE_1;
            break;
        case 2:
            spRen.material.color = PHASE_2;
            break;
        case 3:
            spRen.material.color = PHASE_3;
            break;
        case 4:
            spRen.material.color = PHASE_4;
            break;
        default:
            spRen.material.color = PHASE_0;
            break;
        }
    }

    public void EnableClick(bool _canclick)
    {
        canClick = _canclick;
    }

    public void OnPointerClick()
    {
        if (!canClick) return;

        //anim.SetTrigger("Click");
        OnClickTile.Invoke(id);   //  登録されているイベントを実行する
    }

    public void OnTileFlip()
    {
        phase++;
    }

    public void RerverseFlip()
    {
        phase--;
    }

    public void PlayFlipAnim(bool isPhaseUp = true)
    {
        anim.SetTrigger("Flip");
        if (isPhaseUp)
        {
            OnTileFlip();
        }
    }

    public void PlayReverseFlipAnim(bool isPhasedown = true)
    {
        anim.SetTrigger("ReverseFlip");
        if (isPhasedown)
        {
            RerverseFlip();
        }
    }

    public void PlayAppearanceAnim()
    {
        anim.SetTrigger("Appearance");
    }

    public void PlaySuccessAnim()
    {
        anim.SetTrigger("Success");
    }

    public void PlayClickAnim()
    {
        anim.SetTrigger("Click");
    }
}
