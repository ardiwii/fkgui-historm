using System.Text;

[System.Serializable]
public class Avatar
{
    public byte gender; //1: female, 2: male
    public byte body;
    public byte shirt;
    public byte shirtColor;
    public byte hair;
    public byte hairColor;
    public byte acc;

    public Avatar(string avatarStrData)
    {
        gender = byte.Parse(avatarStrData.Substring(0, 1));
        body = byte.Parse(avatarStrData.Substring(1, 1));
        shirt = byte.Parse(avatarStrData.Substring(2, 2));
        shirtColor = byte.Parse(avatarStrData.Substring(4, 1));
        hair = byte.Parse(avatarStrData.Substring(5, 2));
        hairColor = byte.Parse(avatarStrData.Substring(7, 1));
        acc = byte.Parse(avatarStrData.Substring(8, 2));
    }

    public Avatar(Avatar copyAvatar)
    {
        gender = copyAvatar.gender;
        body = copyAvatar.body;
        shirt = copyAvatar.shirt;
        shirtColor = copyAvatar.shirtColor;
        hair = copyAvatar.hair;
        hairColor = copyAvatar.hairColor;
        acc = copyAvatar.acc;
    }

    public void Reset()
    {
        body = 0;
        shirt = 0;
        shirtColor = 0;
        hair = 0;
        hairColor = 0;
        acc = 0;
    }

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append(gender);
        strBuilder.Append(body);
        string shirtStr = shirt < 10 ? "0" + shirt : shirt.ToString();
        strBuilder.Append(shirtStr);
        strBuilder.Append(shirtColor);
        string hairStr = hair < 10 ? "0" + hair : hair.ToString();
        strBuilder.Append(hairStr);
        strBuilder.Append(hairColor);
        string accStr = acc < 10 ? "0" + acc : acc.ToString();
        strBuilder.Append(accStr);
        return strBuilder.ToString();
    }
}
