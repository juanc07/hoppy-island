#pragma strict

function Start () {
yield WaitForSeconds (3);
animation.Play("chop");
yield WaitForSeconds (5.6);
animation.Play("dance");
yield WaitForSeconds (7.9);
 Application.LoadLevel(0);
}

function Update () {

}