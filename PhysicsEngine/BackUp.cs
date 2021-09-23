/*
Collisions.CollisionOut collision1 = Collisions.CheckCollision(this, collisions);

double xMultiplier = 1;
double yMultiplier = 1;

bool isCollision1 = false;
Collisions.CollisionArea cArea1 = 0;
PhysicalObject collisionObj1 = null;

bool isCollision2 = false;
Collisions.CollisionArea cArea2 = 0;
PhysicalObject collisionObj2 = null;

if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Left)
{
    Collisions.CollisionOut collision2 = Collisions.CheckCollision(
        X + (VelocityX * collision1.Collision.Percent),
        Y + (VelocityY * collision1.Collision.Percent),
        Width,
        Height,
        0,
        VelocityY * (1 - collision1.Collision.Percent),
        collision1.AvailableCollision,
        this);

    xMultiplier = collision1.Collision.Percent;
    yMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);

    if (collision1.Collision.Object != CollisionX)
    {
        isCollision1 = true;
        cArea1 = collision1.Collision.CollisionArea;
        collisionObj1 = collision1.Collision.Object;
        CollisionX = collisionObj1;
    }

    if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Top || collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
        && collision2.Collision.Object != CollisionY)
    {
        isCollision2 = true;
        cArea2 = collision2.Collision.CollisionArea;
        collisionObj2 = collision2.Collision.Object;
        CollisionY = collisionObj2;
    }
}
else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
{
    Collisions.CollisionOut collision2 = Collisions.CheckCollision(
        X + (VelocityX * collision1.Collision.Percent),
        Y + (VelocityY * collision1.Collision.Percent),
        Width,
        Height,
        0,
        VelocityY * (1 - collision1.Collision.Percent),
        collision1.AvailableCollision,
        this);

    xMultiplier = collision1.Collision.Percent;
    yMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);

    if (collision1.Collision.Object != CollisionX)
    {
        isCollision1 = true;
        cArea1 = collision1.Collision.CollisionArea;
        collisionObj1 = collision1.Collision.Object;
        CollisionX = collisionObj1;
    }

    if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Top || collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
        && collision2.Collision.Object != CollisionY)
    {
        isCollision2 = true;
        cArea2 = collision2.Collision.CollisionArea;
        collisionObj2 = collision2.Collision.Object;
        CollisionY = collisionObj2;
    }
}
else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Top)
{
    Collisions.CollisionOut collision2 = Collisions.CheckCollision(
        X + (VelocityX * collision1.Collision.Percent),
        Y + (VelocityY * collision1.Collision.Percent),
        Width,
        Height,
        VelocityX * (1 - collision1.Collision.Percent),
        0,
        collision1.AvailableCollision,
        this);

    xMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);
    yMultiplier = collision1.Collision.Percent;

    if (collision1.Collision.Object != CollisionY)
    {
        isCollision1 = true;
        cArea1 = collision1.Collision.CollisionArea;
        collisionObj1 = collision1.Collision.Object;
        CollisionY = collisionObj1;
    }

    if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Left || collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
        && collision2.Collision.Object != CollisionX)
    {
        isCollision2 = true;
        cArea2 = collision2.Collision.CollisionArea;
        collisionObj2 = collision2.Collision.Object;
        CollisionX = collisionObj2;
    }
}
else if (collision1.Collision.CollisionArea == Collisions.CollisionArea.Bottom)
{
    Collisions.CollisionOut collision2 = Collisions.CheckCollision(
        X + (VelocityX * collision1.Collision.Percent),
        Y + (VelocityY * collision1.Collision.Percent),
        Width,
        Height,
        VelocityX * (1 - collision1.Collision.Percent),
        0,
        collision1.AvailableCollision,
        this);

    xMultiplier = collision1.Collision.Percent + ((1 - collision1.Collision.Percent) * collision2.Collision.Percent);
    yMultiplier = collision1.Collision.Percent;

    if (collision1.Collision.Object != CollisionY)
    {
        isCollision1 = true;
        cArea1 = collision1.Collision.CollisionArea;
        collisionObj1 = collision1.Collision.Object;
        CollisionY = collisionObj1;
    }

    if ((collision2.Collision.CollisionArea == Collisions.CollisionArea.Left || collision1.Collision.CollisionArea == Collisions.CollisionArea.Right)
        && collision2.Collision.Object != CollisionX)
    {
        isCollision2 = true;
        cArea2 = collision2.Collision.CollisionArea;
        collisionObj2 = collision2.Collision.Object;
        CollisionX = collisionObj1;
    }
}
else
{
    CollisionX = null;
    CollisionY = null;
}

double newX = X + (VelocityX * xMultiplier);
double newY = Y + (VelocityY * yMultiplier);

// Apply Location
X = newX;
Y = newY;

// Trigger Collision Events
if (isCollision1)
{
    OnCollision?.Invoke(this, new CollisionEventArgs(new RectangleD(newX, newY, Width, height), cArea1, collisionObj1, VelocityX, VelocityY));
}
if (isCollision2)
{
    OnCollision?.Invoke(this, new CollisionEventArgs(new RectangleD(newX, newY, Width, height), cArea2, collisionObj2, VelocityX, VelocityY));
}

// Futher Physics Calculations
if (xMultiplier != 1)
{
    VelocityX = 0;
}
if (yMultiplier != 1)
{
    VelocityY = 0;
}
*/