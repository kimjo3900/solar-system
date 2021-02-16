using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
	private Button button;
	public Sprite play, pause;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ChangeImage() {
		if (button.image.sprite == pause)
			button.image.sprite = play;
		else
			button.image.sprite = pause;
	}
}
