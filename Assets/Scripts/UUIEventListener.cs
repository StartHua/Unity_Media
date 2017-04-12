using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UUIEventListener : MonoBehaviour,
                                IPointerClickHandler,
                                IPointerDownHandler,
                                IPointerEnterHandler,
                                IPointerExitHandler,
                                IPointerUpHandler,
                                ISelectHandler,
                                IUpdateSelectedHandler,
                                IDeselectHandler,
                                IBeginDragHandler,
                                IDragHandler,
                                IEndDragHandler,
                                IDropHandler,
                                IScrollHandler,
                                IMoveHandler
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDeSelect;
    public VoidDelegate onBeginDrag;//添加
    public VoidDelegate onDrag;
    public VoidDelegate onDragEnd;
    public VoidDelegate onDrop;
    public VoidDelegate onScroll;
    public VoidDelegate onMove;

    public object parameter;

    //放大缩小
    public bool showEffect = false;
    public Vector3 scale = Vector3.one;

    //添加(注意参数)
    public delegate void ObjDelegate(GameObject go, PointerEventData eventData,object parameter);
    public ObjDelegate onBeginDragWithObj;
    public ObjDelegate onDragWithObj;
    public ObjDelegate onDragEndWithObj;

    public void OnPointerClick(PointerEventData eventData) { if (onClick != null) onClick(gameObject); }
    public void OnPointerDown(PointerEventData eventData) {
        if (onDown != null) onDown(gameObject);
        if (showEffect && gameObject != null)
        {
            gameObject.transform.localScale = scale * 1.2f;
        }
            
    }
    public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject); }
    public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject); }
    public void OnPointerUp(PointerEventData eventData) {
        if (onUp != null) onUp(gameObject);
        if (showEffect && gameObject != null)
        {
            gameObject.transform.localScale = scale;
        }
    }
    public void OnSelect(BaseEventData eventData) { if (onSelect != null) onSelect(gameObject); }
    public void OnUpdateSelected(BaseEventData eventData) { if (onUpdateSelect != null) onUpdateSelect(gameObject); }
    public void OnDeselect(BaseEventData eventData) { if (onDeSelect != null) onDeSelect(gameObject); }
   
    public void OnBeginDrag(PointerEventData eventData) {
        if (onBeginDrag != null) 
            onBeginDrag(gameObject);
        if (onBeginDragWithObj != null)
        {
            onBeginDragWithObj(gameObject, eventData, parameter);
        }
    }
    public void OnDrag(PointerEventData eventData) { 
        if (onDrag != null) 
            onDrag(gameObject);
        if (onDragWithObj != null)
            onDragWithObj(gameObject , eventData,parameter);
    }
    public void OnEndDrag(PointerEventData eventData) { 
        if (onDragEnd != null) 
            onDragEnd(gameObject);
        if (onDragEndWithObj != null)
            onDragEndWithObj(gameObject , eventData,parameter);
    }
    
    public void OnDrop(PointerEventData eventData) { if (onDrop != null) onDrop(gameObject); }
    public void OnScroll(PointerEventData eventData) { if (onScroll != null) onScroll(gameObject); }
    public void OnMove(AxisEventData eventData) { if (onMove != null) onMove(gameObject); }

    static public UUIEventListener Get(GameObject go,bool isShowEffect = true)
    {
        UUIEventListener listener = go.GetComponent<UUIEventListener>();
        if (listener == null) listener = go.AddComponent<UUIEventListener>();
        listener.showEffect = isShowEffect;
        listener.scale = go.transform.localScale;
        return listener;
    }

    static public UUIEventListener Get(Transform tf)
    {
        GameObject go = tf.gameObject;
        UUIEventListener listener = go.GetComponent<UUIEventListener>();
        if (listener == null) listener = go.AddComponent<UUIEventListener>();
        return listener;
    }
   
}