using UnityEngine;

public class Tiling : MonoBehaviour
{

    private const int LEFT = -1;
	private const int RIGHT = 1;

	public int offsetX = 2;			// the offset so that we don't get any weird errors

    // these are used for checking if we need to instantiate stuff
	public bool hasARightBuddy;
	public bool hasALeftBuddy;

    public bool reverseScale;	// used if the object is not tilable

	private float spriteWidth;		// the width of our element
	private Camera cam;
	private Transform myTransform;

    void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // does it still need buddies? If not do nothing
		if (!hasALeftBuddy || !hasARightBuddy) {
            // calculate the cameras extend (half the width) of what the camera can see in world coordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

            // calculate the x position where the camera can see the edge of the sprite (element)
            var position = myTransform.position;
            float edgeVisiblePositionRight = (position.x + spriteWidth/2) - camHorizontalExtend;
   			float edgeVisiblePositionLeft = (position.x - spriteWidth/2) + camHorizontalExtend;

            // checking if we can see the edge of the element and then calling MakeNewBuddy if we can
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
			{
				MakeNewBuddy (RIGHT);
				hasARightBuddy = true;
			}
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasALeftBuddy)
			{
				MakeNewBuddy (LEFT);
				hasALeftBuddy = true;
			}
        }
    }

    // a function that creates a buddy on the side required
    void MakeNewBuddy (int rightOrLeft) {
        // calculating the new position for our new buddy
        var position = myTransform.position;
        Vector3 newPosition = new Vector3 (position.x + spriteWidth * rightOrLeft,
                                           position.y, position.z);
        // instantating our new body and storing him in a variable
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation);

		// if not tilable let's reverse the x size og our object to get rid of ugly seams
		if (reverseScale)
		{
			var localScale = newBuddy.localScale;
			localScale = new Vector3 (localScale.x*-1, localScale.y, localScale.z);
			newBuddy.localScale = localScale;
		}      
        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else {
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
    }
}