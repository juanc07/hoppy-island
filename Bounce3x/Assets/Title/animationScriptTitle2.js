#pragma strict

function Start () {
animation.Stop();
yield WaitForSeconds (3);
animation.Play("titlefight");

}

function Update () {

}