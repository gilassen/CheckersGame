using System.Drawing;
using System.Windows.Forms;
using System;

public class PieceAnimator
{
    private Timer m_MoveTimer;
    private Timer m_KingTimer;
    private Button m_AnimatedButton;
    private Point[] m_PathPoints;
    private int m_CurrentStep;
    private Action m_OnCompleted;
    private Form m_ParentForm;
    private int m_KingAnimationPhase = 0;
    private Button m_KingButton;
    private Color m_OriginalBackColor;
    private Size m_OriginalSize;
    private Point m_OriginalLocation;
    private Font m_OriginalFont;

    public PieceAnimator(Form i_Form)
    {
        m_ParentForm = i_Form;
        m_MoveTimer = new Timer { Interval = 50 };

        m_MoveTimer.Tick += moveTimerTick;
        m_KingTimer = new Timer { Interval = 100 };

        m_KingTimer.Tick += kingTimerTick;
    }

    public void StartMoveAnimation(Button i_SourceButton, Point i_Start, Point i_End, Action i_OnComplete = null)
    {
        m_CurrentStep = 0;
        m_OnCompleted = i_OnComplete;
        m_PathPoints = calculatePathPoints(i_Start, i_End, 10);
        m_AnimatedButton = cloneButton(i_SourceButton);
        m_ParentForm.Controls.Add(m_AnimatedButton);
        m_AnimatedButton.BringToFront();
        i_SourceButton.Text = "";
        setButtonToFront(m_AnimatedButton);
        m_MoveTimer.Start();
    }

    private void moveTimerTick(object sender, EventArgs e)
    {
        if (m_CurrentStep < m_PathPoints.Length)
        {
            m_AnimatedButton.Location = m_PathPoints[m_CurrentStep];
            m_AnimatedButton.BringToFront();
            m_CurrentStep++;
        }
        else
        {
            m_MoveTimer.Stop();
            m_ParentForm.Controls.Remove(m_AnimatedButton);
            m_AnimatedButton.Dispose();
            m_OnCompleted?.Invoke();
        }
    }

    public void StartKingAnimation(Button i_Button, Action i_OnComplete = null)
    {
        m_KingButton = i_Button;
        m_KingAnimationPhase = 0;
        m_OnCompleted = i_OnComplete;
        saveOriginalKingAppearance(i_Button);
        m_KingTimer.Start();
    }

    private void kingTimerTick(object sender, EventArgs e)
    {
        if (m_KingAnimationPhase < 6)
        {
            adjustKingAppearance(m_KingButton, m_KingAnimationPhase % 2 == 0 ? 10 : 0, m_KingAnimationPhase % 2 == 0 ? 2 : 0);
        }
        else if (m_KingAnimationPhase < 12)
        {
            m_KingButton.BackColor = m_KingAnimationPhase % 2 == 0 ? Color.Gold : m_OriginalBackColor;
        }
        else
        {
            m_KingTimer.Stop();
            m_KingButton.BackColor = m_OriginalBackColor;
            m_OnCompleted?.Invoke();
        }
        m_KingAnimationPhase++;
    }

    private Point[] calculatePathPoints(Point i_Start, Point i_End, int i_TotalSteps)
    {
        Point[] path = new Point[i_TotalSteps + 1];

        for (int i = 0; i <= i_TotalSteps; i++)
        {
            double progress = (double)i / i_TotalSteps;
            path[i] = new Point(
                (int)(i_Start.X + (i_End.X - i_Start.X) * progress),
                (int)(i_Start.Y + (i_End.Y - i_Start.Y) * progress)
            );
        }

        return path;
    }

    private Button cloneButton(Button i_SourceButton)
    {
        return new Button
        {
            Size = i_SourceButton.Size,
            Location = i_SourceButton.Location,
            Text = i_SourceButton.Text,
            Font = i_SourceButton.Font,
            FlatStyle = FlatStyle.Flat,
            BackColor = i_SourceButton.BackColor,
            ForeColor = i_SourceButton.ForeColor,
        };
    }

    private void setButtonToFront(Button i_Button)
    {
        for (int i = 0; i < m_ParentForm.Controls.Count; i++)
        {
            if (m_ParentForm.Controls[i] == i_Button)
            {
                m_ParentForm.Controls.SetChildIndex(i_Button, 0);
                break;
            }
        }
    }

    private void saveOriginalKingAppearance(Button i_Button)
    {
        m_OriginalBackColor = i_Button.BackColor;
        m_OriginalSize = i_Button.Size;
        m_OriginalLocation = i_Button.Location;
        m_OriginalFont = i_Button.Font;
    }

    private void adjustKingAppearance(Button i_Button, int i_SizeOffset, int i_FontSizeOffset)
    {
        i_Button.Size = new Size(m_OriginalSize.Width + i_SizeOffset, m_OriginalSize.Height + i_SizeOffset);
        i_Button.Location = new Point(m_OriginalLocation.X - (i_SizeOffset / 2), m_OriginalLocation.Y - (i_SizeOffset / 2));
        i_Button.Font = new Font(i_Button.Font.FontFamily, m_OriginalFont.Size + i_FontSizeOffset, FontStyle.Bold);
        
        i_Button.BringToFront();
    }
}
