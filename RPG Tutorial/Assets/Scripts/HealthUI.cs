using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to use 'Image' type vars


[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour
{
    public GameObject UIPrefab; //red and yellow health bar UI
    public Transform target; //position want UI to follow

    private Transform spawnedUI;
    private Image healthSlider; //green part of hp bar we fill based on how much hp character has
    private Transform cam;

    //for dissapearing HPUI after some time if no damage taken:
    private float visibleTime = 5f; //
    private float lastMadeVisibleTime;

    // Start is called before the first frame update
    void Start()
    {

        cam = Camera.main.transform;

        //find canvas that's rendered in the world space + parent UIprefab to it:
        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                //instantiate 'UIPrefab' as invisible and parent it to this particular 'canvas':
                spawnedUI = Instantiate(UIPrefab, canvas.transform).transform;
                spawnedUI.gameObject.SetActive(false);

                //set health slider to top health bar:
                healthSlider = spawnedUI.GetChild(0).GetComponent<Image>(); //yellow health bar's image comp grabbed

                break; //stop looking w/ world space canvas found
            }
        }

        //subscribe method to a callback created and called in Character Stats script:
        GetComponent<CharacterStats>().OnHealthChangedCallback += OnHealthChanged;
    }

    private void OnHealthChanged(int maxHP, int currHP)
    {
        if (spawnedUI != null)
        {
            //hp bar made visible:
            spawnedUI.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            //calc new percent of hp have:
            float healthPercent = currHP / (float)maxHP; //only have to cast one val, other is cast thru coercion

            //apply new hp to health bar:
            healthSlider.fillAmount = healthPercent;

            //destroy HPUI w/ die:
            if (currHP <= 0)
            {
                Destroy(spawnedUI.gameObject);
            }
        }
    }

    //not 'Update()' to make sure target.position is updated before grabbing it:
    void LateUpdate()
    {
        if (spawnedUI != null)
        {
            //deactivate HPUI if not taken damage within 'visibleTime':
            if(Time.time - lastMadeVisibleTime > visibleTime)
            {
                spawnedUI.gameObject.SetActive(false);
            }

            //constantly track HPUI to desired spot above character, facing camera:
            spawnedUI.position = target.position;
            spawnedUI.forward = -cam.forward;
        }
    }
}
